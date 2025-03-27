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

        // Create a PhoneNumberType from DTO
        public async Task<PhoneNumberTypeDTO> CreatePhoneNumberTypeAsync(PhoneNumberTypeDTO phoneNumberTypeDto)
        {
            // Validate incoming DTO data
            if (phoneNumberTypeDto == null)
                throw new ArgumentNullException(nameof(phoneNumberTypeDto));

            var phoneNumberType = new PhoneNumberType
            {
                Name = phoneNumberTypeDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PhoneNumberTypes.Add(phoneNumberType);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            phoneNumberTypeDto.Id = phoneNumberType.Id;
            phoneNumberTypeDto.CreatedAt = phoneNumberType.CreatedAt;
            phoneNumberTypeDto.UpdatedAt = phoneNumberType.UpdatedAt;

            return phoneNumberTypeDto;
        }

        // Get a PhoneNumberType by ID and map to DTO
        public async Task<PhoneNumberTypeDTO?> GetPhoneNumberTypeByIdAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes
                .FirstOrDefaultAsync(pnt => pnt.Id == id && pnt.Removed == false);  // Ensure it's not marked as removed

            if (phoneNumberType == null)
                return null;

            // Map to DTO
            var phoneNumberTypeDto = new PhoneNumberTypeDTO
            {
                Id = phoneNumberType.Id,
                Name = phoneNumberType.Name,
                Removed = phoneNumberType.Removed,
                CreatedAt = phoneNumberType.CreatedAt,
                UpdatedAt = phoneNumberType.UpdatedAt
            };

            return phoneNumberTypeDto;
        }

        // Get all PhoneNumberTypes and map to DTOs
        public async Task<List<PhoneNumberTypeDTO>> GetAllPhoneNumberTypesAsync()
        {
            var phoneNumberTypes = await _context.PhoneNumberTypes
                .Where(pnt => pnt.Removed == false)  // Ensure phone number types are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var phoneNumberTypeDtos = phoneNumberTypes.Select(pnt => new PhoneNumberTypeDTO
            {
                Id = pnt.Id,
                Name = pnt.Name,
                Removed = pnt.Removed,
                CreatedAt = pnt.CreatedAt,
                UpdatedAt = pnt.UpdatedAt
            }).ToList();

            return phoneNumberTypeDtos;
        }

        // Update a PhoneNumberType from DTO
        public async Task<PhoneNumberTypeDTO> UpdatePhoneNumberTypeAsync(int id, PhoneNumberTypeDTO updatedPhoneNumberTypeDto)
        {
            // Validate the incoming DTO data
            if (updatedPhoneNumberTypeDto == null)
                throw new ArgumentNullException(nameof(updatedPhoneNumberTypeDto));

            var existingPhoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (existingPhoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            // Update properties from DTO
            existingPhoneNumberType.Name = updatedPhoneNumberTypeDto.Name;
            existingPhoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(existingPhoneNumberType);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedPhoneNumberTypeDto.Id = existingPhoneNumberType.Id;
            updatedPhoneNumberTypeDto.CreatedAt = existingPhoneNumberType.CreatedAt;
            updatedPhoneNumberTypeDto.UpdatedAt = existingPhoneNumberType.UpdatedAt;

            return updatedPhoneNumberTypeDto;
        }

        // Soft Delete a PhoneNumberType by ID
        public async Task<bool> DeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (phoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            phoneNumberType.Removed = true;  // Mark as removed
            phoneNumberType.UpdatedAt = DateTime.UtcNow;

            _context.PhoneNumberTypes.Update(phoneNumberType);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete a PhoneNumberType
        public async Task<bool> PermanentlyDeletePhoneNumberTypeAsync(int id)
        {
            var phoneNumberType = await _context.PhoneNumberTypes.FindAsync(id);

            if (phoneNumberType == null)
                throw new KeyNotFoundException("Phone Number Type not found.");

            _context.PhoneNumberTypes.Remove(phoneNumberType);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
