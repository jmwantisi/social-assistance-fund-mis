using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class PhoneNumberTypeService
    {
        private readonly ApplicationDbContext _context;

        public PhoneNumberTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a PhoneNumberType
        public async Task<PhoneNumberType> CreatePhoneNumberTypeAsync(PhoneNumberType phoneNumberType)
        {
            // Validate incoming data
            if (phoneNumberType == null)
                throw new ArgumentNullException(nameof(phoneNumberType));

            phoneNumberType.CreatedAt = DateTime.UtcNow;
            phoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Add(phoneNumberType);
            await _context.SaveChangesAsync();

            return phoneNumberType;
        }

        // Get a PhoneNumberType by ID
        public async Task<PhoneNumberType?> GetPhoneNumberTypeByIdAsync(int id)
        {
            return await _context.PhoneNumberTypes
                .FirstOrDefaultAsync(pnt => pnt.Id == id && pnt.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all PhoneNumberTypes
        public async Task<List<PhoneNumberType>> GetAllPhoneNumberTypesAsync()
        {
            return await _context.PhoneNumberTypes
                .Where(pnt => pnt.Removed == false)  // Ensure phone number types are not marked as removed
                .ToListAsync();
        }

        // Update a PhoneNumberType
        public async Task<PhoneNumberType> UpdatePhoneNumberTypeAsync(int id, PhoneNumberType updatedPhoneNumberType)
        {
            // Validate the incoming updated phone number type data
            if (updatedPhoneNumberType == null)
                throw new ArgumentNullException(nameof(updatedPhoneNumberType));

            var existingPhoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (existingPhoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            // Update properties
            existingPhoneNumberType.Name = updatedPhoneNumberType.Name;
            existingPhoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(existingPhoneNumberType);
            await _context.SaveChangesAsync();

            return existingPhoneNumberType;
        }

        // Soft Delete a PhoneNumberType
        public async Task DeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (phoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            phoneNumberType.Removed = true;  // Mark as removed
            phoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(phoneNumberType);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a PhoneNumberType
        public async Task PermanentlyDeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (phoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            _context.PhoneNumberTypes.Remove(phoneNumberType);
            await _context.SaveChangesAsync();
        }
    }
}
