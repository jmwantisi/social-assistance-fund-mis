using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace socialAssistanceFundMIS.Services
{
    public interface IApplicantService
    {
        Task<ApplicantDTO> CreateApplicantAsync(ApplicantDTO applicantDto, List<string> phoneNumbers);
        Task<ApplicantDTO> GetApplicantByIdAsync(int id);
        Task<List<ApplicantDTO>> GetAllApplicantsAsync();
        Task<ApplicantDTO> UpdateApplicantAsync(int id, ApplicantDTO applicantDto, List<string> phoneNumbers);
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
        public async Task<ApplicantDTO> CreateApplicantAsync(ApplicantDTO applicantDto, List<string> phoneNumbers)
        {
            if (applicantDto == null)
                throw new ArgumentNullException(nameof(applicantDto));

            // Create the applicant
            var applicant = new Applicant
            {
                FirstName = applicantDto.FirstName,
                MiddleName = applicantDto.MiddleName,
                LastName = applicantDto.LastName,
                SexId = applicantDto.SexId,
                Dob = applicantDto.Dob,
                MaritialStatusId = applicantDto.MaritialStatusId,
                CountyId = applicantDto.CountyId,
                SubCountyId = applicantDto.SubCountyId,
                LocationId = applicantDto.LocationId,
                SubLocationId = applicantDto.SubLocationId,
                VillageId = applicantDto.VillageId,
                IdentityCardNumber = applicantDto.IdentityCardNumber,
                PostalAddress = applicantDto.PostalAddress,
                PhysicalAddress = applicantDto.PhysicalAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            // Add phone numbers if provided
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

            // Map to ApplicantDTO
            return MapToApplicantDTO(applicant);
        }

        // Get a single applicant by ID with phone numbers
        public async Task<ApplicantDTO> GetApplicantByIdAsync(int id)
        {
            var applicant = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed)) // Only active phone numbers
                .Include(a => a.Sex)
                .Include(a => a.MaritialStatus)
                .Include(a => a.County)
                .Include(a => a.SubCounty)
                .Include(a => a.Location)
                .Include(a => a.SubLocation)
                .Include(a => a.Village)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed); // Only active applicants

            return applicant == null ? null : MapToApplicantDTO(applicant);
        }

        // Get all applicants with their active phone numbers
        public async Task<List<ApplicantDTO>> GetAllApplicantsAsync()
        {
            var applicants = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed)) // Only active phone numbers
                .Where(a => !a.Removed) // Only active applicants
                .ToListAsync();

            return applicants.Select(a => MapToApplicantDTO(a)).ToList();
        }

        // Update an existing applicant and manage phone numbers
        public async Task<ApplicantDTO> UpdateApplicantAsync(int id, ApplicantDTO applicantDto, List<string> phoneNumbers)
        {
            var existingApplicant = await _context.Applicants
                .Include(a => a.PhoneNumbers)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (existingApplicant == null)
                return null;

            // Update applicant details
            existingApplicant.FirstName = applicantDto.FirstName;
            existingApplicant.LastName = applicantDto.LastName;
            existingApplicant.SexId = applicantDto.SexId;
            existingApplicant.Dob = applicantDto.Dob;
            existingApplicant.MaritialStatusId = applicantDto.MaritialStatusId;
            existingApplicant.CountyId = applicantDto.CountyId;
            existingApplicant.SubCountyId = applicantDto.SubCountyId;
            existingApplicant.LocationId = applicantDto.LocationId;
            existingApplicant.SubLocationId = applicantDto.SubLocationId;
            existingApplicant.VillageId = applicantDto.VillageId;
            existingApplicant.IdentityCardNumber = applicantDto.IdentityCardNumber;
            existingApplicant.PostalAddress = applicantDto.PostalAddress;
            existingApplicant.PhysicalAddress = applicantDto.PhysicalAddress;
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
            return MapToApplicantDTO(existingApplicant);
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

        // Helper method to map an Applicant entity to an ApplicantDTO
        private ApplicantDTO MapToApplicantDTO(Applicant applicant)
        {
            return new ApplicantDTO
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                MiddleName = applicant.MiddleName,
                LastName = applicant.LastName,
                SexId = applicant.SexId,
                SexName = applicant.Sex?.Name,
                Dob = applicant.Dob,
                MaritialStatusId = applicant.MaritialStatusId,
                MaritialStatusName = applicant.MaritialStatus?.Name,
                CountyId = applicant.CountyId,
                CountyName = applicant.County?.Name,
                SubCountyId = applicant.SubCountyId,
                SubCountyName = applicant.SubCounty?.Name,
                LocationId = applicant.LocationId,
                LocationName = applicant.Location?.Name,
                SubLocationId = applicant.SubLocationId,
                SubLocationName = applicant.SubLocation?.Name,
                VillageId = applicant.VillageId,
                VillageName = applicant.Village?.Name,
                IdentityCardNumber = applicant.IdentityCardNumber,
                PhoneNumbers = applicant.PhoneNumbers
                    .Where(p => !p.Removed)
                    .Select(p => new ApplicantPhoneNumberDTO
                    {
                        Id = p.Id,
                        PhoneNumber = p.PhoneNumber,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    }).ToList(),
                PostalAddress = applicant.PostalAddress,
                PhysicalAddress = applicant.PhysicalAddress,
                Removed = applicant.Removed,
                CreatedAt = applicant.CreatedAt,
                UpdatedAt = applicant.UpdatedAt
            };
        }
    }
}
