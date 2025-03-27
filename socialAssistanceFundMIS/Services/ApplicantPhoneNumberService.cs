using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class ApplicantPhoneNumberService
    {
        private readonly ApplicationDbContext _context;

        public ApplicantPhoneNumberService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create an ApplicantPhoneNumber
        public async Task<ApplicantPhoneNumber> CreateApplicantPhoneNumberAsync(ApplicantPhoneNumber applicantPhoneNumber)
        {
            // Validate incoming data
            if (applicantPhoneNumber == null)
                throw new ArgumentNullException(nameof(applicantPhoneNumber));

            applicantPhoneNumber.CreatedAt = DateTime.UtcNow;
            applicantPhoneNumber.UpdatedAt = DateTime.UtcNow;

            _context.ApplicantPhoneNumbers.Add(applicantPhoneNumber);
            await _context.SaveChangesAsync();

            return applicantPhoneNumber;
        }

        // Get an ApplicantPhoneNumber by ID
        public async Task<ApplicantPhoneNumber?> GetApplicantPhoneNumberByIdAsync(int id)
        {
            return await _context.ApplicantPhoneNumbers
                .Include(apn => apn.Applicant)  // Include related Applicant data
                .Include(apn => apn.PhoneNumberType) // Include related PhoneNumberType data
                .FirstOrDefaultAsync(apn => apn.Id == id && apn.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all ApplicantPhoneNumbers
        public async Task<List<ApplicantPhoneNumber>> GetAllApplicantPhoneNumbersAsync()
        {
            return await _context.ApplicantPhoneNumbers
                .Include(apn => apn.Applicant)
                .Include(apn => apn.PhoneNumberType)
                .Where(apn => apn.Removed == false)  // Ensure records are not marked as removed
                .ToListAsync();
        }

        // Update an ApplicantPhoneNumber
        public async Task<ApplicantPhoneNumber> UpdateApplicantPhoneNumberAsync(int id, ApplicantPhoneNumber updatedApplicantPhoneNumber)
        {
            // Validate the incoming updated phone number data
            if (updatedApplicantPhoneNumber == null)
                throw new ArgumentNullException(nameof(updatedApplicantPhoneNumber));

            var existingPhoneNumber = await _context.ApplicantPhoneNumbers.FindAsync(id);

            if (existingPhoneNumber == null)
                throw new KeyNotFoundException("ApplicantPhoneNumber not found.");

            // Update properties
            existingPhoneNumber.ApplicantId = updatedApplicantPhoneNumber.ApplicantId;
            existingPhoneNumber.PhoneNumber = updatedApplicantPhoneNumber.PhoneNumber;
            existingPhoneNumber.PhoneNumberTypeId = updatedApplicantPhoneNumber.PhoneNumberTypeId;
            existingPhoneNumber.UpdatedAt = DateTime.UtcNow;

            _context.ApplicantPhoneNumbers.Update(existingPhoneNumber);
            await _context.SaveChangesAsync();

            return existingPhoneNumber;
        }

        // Soft Delete an ApplicantPhoneNumber
        public async Task DeleteApplicantPhoneNumberAsync(int id)
        {
            var applicantPhoneNumber = await _context.ApplicantPhoneNumbers.FindAsync(id);

            if (applicantPhoneNumber == null)
                throw new KeyNotFoundException("ApplicantPhoneNumber not found.");

            applicantPhoneNumber.Removed = true;  // Mark as removed
            applicantPhoneNumber.UpdatedAt = DateTime.UtcNow;

            _context.ApplicantPhoneNumbers.Update(applicantPhoneNumber);
            await _context.SaveChangesAsync();
        }

        // Permanently delete an ApplicantPhoneNumber
        public async Task PermanentlyDeleteApplicantPhoneNumberAsync(int id)
        {
            var applicantPhoneNumber = await _context.ApplicantPhoneNumbers.FindAsync(id);

            if (applicantPhoneNumber == null)
                throw new KeyNotFoundException("ApplicantPhoneNumber not found.");

            _context.ApplicantPhoneNumbers.Remove(applicantPhoneNumber);
            await _context.SaveChangesAsync();
        }
    }
}
