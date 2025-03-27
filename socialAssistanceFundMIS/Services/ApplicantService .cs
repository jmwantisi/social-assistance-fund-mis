using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Data;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public interface IApplicantService
    {
        Task<Applicant> CreateApplicantAsync(Applicant applicant, List<string> phoneNumbers);
        Task<Applicant> GetApplicantByIdAsync(int id);
        Task<List<Applicant>> GetAllApplicantsAsync();
        Task<Applicant> UpdateApplicantAsync(int id, Applicant applicant, List<string> phoneNumbers);
        Task<bool> DeleteApplicantAsync(int id);
        Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId);
    }

    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationDbContext _context;

        public ApplicantService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new applicant with associated phone numbers
        public async Task<Applicant> CreateApplicantAsync(Applicant applicant, List<string> phoneNumbers)
        {
            if (applicant == null)
                throw new ArgumentNullException(nameof(applicant));

            // Create the applicant
            applicant.CreatedAt = DateTime.UtcNow;
            applicant.UpdatedAt = DateTime.UtcNow;
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            // Add the phone numbers if provided
            if (phoneNumbers != null && phoneNumbers.Any())
            {
                foreach (var phoneNumber in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        ApplicantId = applicant.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    applicant.PhoneNumbers.Add(newPhoneNumber);
                }

                await _context.SaveChangesAsync();
            }

            return applicant;
        }

        // Get a single applicant by ID with phone numbers
        public async Task<Applicant> GetApplicantByIdAsync(int id)
        {
            var applicant = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed)) // Only active phone numbers
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed); // Only active applicants

            return applicant;
        }

        // Get all applicants with their active phone numbers
        public async Task<List<Applicant>> GetAllApplicantsAsync()
        {
            var applicants = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed)) // Only active phone numbers
                .Where(a => !a.Removed) // Only active applicants
                .ToListAsync();

            return applicants;
        }

        // Update an existing applicant and manage phone numbers
        public async Task<Applicant> UpdateApplicantAsync(int id, Applicant applicant, List<string> phoneNumbers)
        {
            var existingApplicant = await _context.Applicants
                .Include(a => a.PhoneNumbers)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (existingApplicant == null)
                return null;

            // Update applicant details
            existingApplicant.FirstName = applicant.FirstName;
            existingApplicant.LastName = applicant.LastName;
            existingApplicant.SexId = applicant.SexId;
            existingApplicant.Dob = applicant.Dob;
            existingApplicant.MaritialStatusId = applicant.MaritialStatusId;
            existingApplicant.CountyId = applicant.CountyId;
            existingApplicant.SubCountyId = applicant.SubCountyId;
            existingApplicant.LocationId = applicant.LocationId;
            existingApplicant.SubLocationId = applicant.SubLocationId;
            existingApplicant.VillageId = applicant.VillageId;
            existingApplicant.IdentityCardNumber = applicant.IdentityCardNumber;
            existingApplicant.PostalAddress = applicant.PostalAddress;
            existingApplicant.PhysicalAddress = applicant.PhysicalAddress;
            existingApplicant.UpdatedAt = DateTime.UtcNow;

            // Update phone numbers
            if (phoneNumbers != null && phoneNumbers.Any())
            {
                // Remove old phone numbers (optional: if they are to be replaced)
                existingApplicant.PhoneNumbers.Clear();

                // Add new phone numbers
                foreach (var phoneNumber in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        ApplicantId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    existingApplicant.PhoneNumbers.Add(newPhoneNumber);
                }
            }

            await _context.SaveChangesAsync();
            return existingApplicant;
        }

        // Soft delete an applicant (set Removed to true)
        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (applicant == null)
                return false;

            // Soft delete the applicant
            applicant.Removed = true;
            applicant.UpdatedAt = DateTime.UtcNow;

            // Soft delete the applicant's phone numbers
            foreach (var phoneNumber in applicant.PhoneNumbers)
            {
                phoneNumber.Removed = true;
                phoneNumber.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Soft delete a phone number (individual delete)
        public async Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId)
        {
            var phoneNumberEntity = await _context.ApplicantPhoneNumbers
                .FirstOrDefaultAsync(p => p.Id == phoneNumberId && p.ApplicantId == applicantId && !p.Removed);

            if (phoneNumberEntity == null)
                return false;

            // Soft delete the phone number by setting Removed flag to true
            phoneNumberEntity.Removed = true;
            phoneNumberEntity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
