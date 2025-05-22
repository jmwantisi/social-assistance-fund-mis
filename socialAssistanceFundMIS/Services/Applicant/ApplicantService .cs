using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Data;
using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.ViewModels;
using socialAssistanceFundMIS.Services.GeographicLocationServices;

namespace socialAssistanceFundMIS.Services.Applicants
{
    public class ApplicantService : IApplicantService
    {
        private readonly ApplicationDbContext _context;

        private readonly IGeographicLocationService _geographicLocationService;

        public ApplicantService(ApplicationDbContext context, IGeographicLocationService geographicLocationService)
        {
            _context = context;
            _geographicLocationService = geographicLocationService;
        }

        public async Task<ApplicantViewModel> CreateApplicantAsync(Applicant applicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers)
        {
            if (applicant == null)
                throw new ArgumentNullException(nameof(applicant));

            applicant.CreatedAt = DateTime.UtcNow;
            applicant.UpdatedAt = DateTime.UtcNow;

            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            if (phoneNumbers != null && phoneNumbers.Any())
            {
                foreach (var (phoneNumber, phoneNumberTypeId) in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        PhoneNumberTypeId = phoneNumberTypeId,
                        ApplicantId = applicant.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.ApplicantPhoneNumbers.Add(newPhoneNumber);
                }
                await _context.SaveChangesAsync();
            }

            // Return the mapped ViewModel instead of the domain entity
            return await MapToViewModelAsync(applicant);
        }

        public async Task<ApplicantViewModel> GetApplicantByIdAsync(int id)
        {
            var applicant = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed))
                .Include(a => a.Sex)
                .Include(a => a.MaritialStatus)
                .Include(a => a.Village)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            return applicant == null ? null : await MapToViewModelAsync(applicant);
        }

        public async Task<List<ApplicantViewModel>> GetAllApplicantsAsync()
        {
            var applicants = await _context.Applicants
                .Include(a => a.PhoneNumbers.Where(p => !p.Removed))
                .ThenInclude(p => p.PhoneNumberType) // Include PhoneNumberType for each phone number
                .Include(a => a.MaritialStatus)
                .Include(a => a.Sex)
                .Include(a => a.Village)
                .Where(a => !a.Removed)
                .ToListAsync();

            var applicantsViewModel = applicants.Select(a => new ApplicantViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Dob = a.Dob,
                SexName = a.Sex?.Name,
                MaritalStatusName = a.MaritialStatus?.Name,
                IdentityCardNumber = a.IdentityCardNumber,
                PostalAddress = a.PostalAddress,
                PhysicalAddress = a.PhysicalAddress,
                VillageId = a.VillageId,
                PhoneNumbersListString = string.Join(", ",
                    a.PhoneNumbers
                        .Where(p => !p.Removed)
                        .Select(p => $"{p.PhoneNumber} ({p.PhoneNumberType?.Name})")
                ),
            }).ToList();

            foreach (var applicant in applicantsViewModel)
            {
                if (applicant.VillageId.HasValue)
                {
                    applicant.Location = await _geographicLocationService.GetVillageHierarchyByIdAsync(applicant.VillageId);
                }
            }

            return applicantsViewModel;
        }


        public async Task<ApplicantViewModel> UpdateApplicantAsync(int id, Applicant updatedApplicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers)
        {
            var existingApplicant = await _context.Applicants
                .Include(a => a.PhoneNumbers)
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (existingApplicant == null)
                return null;

            existingApplicant.FirstName = updatedApplicant.FirstName;
            existingApplicant.LastName = updatedApplicant.LastName;
            existingApplicant.SexId = updatedApplicant.SexId;
            existingApplicant.Dob = updatedApplicant.Dob;
            existingApplicant.MaritialStatusId = updatedApplicant.MaritialStatusId;
            existingApplicant.VillageId = updatedApplicant.VillageId;
            existingApplicant.IdentityCardNumber = updatedApplicant.IdentityCardNumber;
            existingApplicant.PostalAddress = updatedApplicant.PostalAddress;
            existingApplicant.PhysicalAddress = updatedApplicant.PhysicalAddress;
            existingApplicant.UpdatedAt = DateTime.UtcNow;

            // Update phone numbers
            if (phoneNumbers != null && phoneNumbers.Any())
            {
                existingApplicant.PhoneNumbers.Clear();
                foreach (var (phoneNumber, phoneNumberTypeId) in phoneNumbers)
                {
                    var newPhoneNumber = new ApplicantPhoneNumber
                    {
                        PhoneNumber = phoneNumber,
                        PhoneNumberTypeId = phoneNumberTypeId,
                        ApplicantId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    existingApplicant.PhoneNumbers.Add(newPhoneNumber);
                }
            }

            await _context.SaveChangesAsync();

            // Return the updated Applicant as a ViewModel
            return await MapToViewModelAsync(existingApplicant);
        }

        public async Task<bool> DeleteApplicantAsync(int id)
        {
            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(a => a.Id == id && !a.Removed);

            if (applicant == null)
                return false;

            applicant.Removed = true;
            applicant.UpdatedAt = DateTime.UtcNow;

            foreach (var phoneNumber in applicant.PhoneNumbers)
            {
                phoneNumber.Removed = true;
                phoneNumber.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId)
        {
            var phoneNumberEntity = await _context.ApplicantPhoneNumbers
                .FirstOrDefaultAsync(p => p.Id == phoneNumberId && p.ApplicantId == applicantId && !p.Removed);

            if (phoneNumberEntity == null)
                return false;

            phoneNumberEntity.Removed = true;
            phoneNumberEntity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Helper method to map Applicant to ApplicantViewModel
        private async Task<ApplicantViewModel> MapToViewModelAsync(Applicant applicant)
        {
            var gender = await _context.Sexes
                .FirstOrDefaultAsync(a => a.Id == applicant.SexId);

            var village = await _context.GeographicLocations
                .FirstOrDefaultAsync(a => a.Id == applicant.VillageId);

            return new ApplicantViewModel
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Dob = applicant.Dob,
                SexName = gender?.Name,
                SexId = gender.Id,
                Email = applicant.Email,
                MaritalStatusName = applicant.MaritialStatus?.Name,
                MaritalStatusId = applicant.MaritialStatusId,
                IdentityCardNumber = applicant.IdentityCardNumber,
                Location = village?.Name,
                VillageId = village?.Id,
                PostalAddress = applicant.PostalAddress,
                PhysicalAddress = applicant.PhysicalAddress,
                PhoneNumbersListString = string.Join(", ",
                    applicant.PhoneNumbers
                        .Where(p => !p.Removed)
                        .Select(p => $"{p.PhoneNumber} ({p.PhoneNumberType?.Name})")
                ),
                PhoneNumbers = applicant.PhoneNumbers
            };

        }

    }
}
