using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class MaritialStatusService
    {
        private readonly ApplicationDbContext _context;

        public MaritialStatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a MaritialStatus from DTO
        public async Task<MaritialStatusDTO> CreateMaritialStatusAsync(MaritialStatusDTO maritialStatusDto)
        {
            // Validate incoming DTO data
            if (maritialStatusDto == null)
                throw new ArgumentNullException(nameof(maritialStatusDto));

            var maritialStatus = new MaritialStatus
            {
                Name = maritialStatusDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MaritalStatuses.Add(maritialStatus);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            maritialStatusDto.Id = maritialStatus.Id;
            maritialStatusDto.CreatedAt = maritialStatus.CreatedAt;
            maritialStatusDto.UpdatedAt = maritialStatus.UpdatedAt;

            return maritialStatusDto;
        }

        // Get a MaritialStatus by ID and map to DTO
        public async Task<MaritialStatusDTO?> GetMaritialStatusByIdAsync(int id)
        {
            var maritialStatus = await _context.MaritalStatuses
                .FirstOrDefaultAsync(ms => ms.Id == id && ms.Removed == false);  // Ensure it's not marked as removed

            if (maritialStatus == null)
                return null;

            // Map to DTO
            var maritialStatusDto = new MaritialStatusDTO
            {
                Id = maritialStatus.Id,
                Name = maritialStatus.Name,
                Removed = maritialStatus.Removed,
                CreatedAt = maritialStatus.CreatedAt,
                UpdatedAt = maritialStatus.UpdatedAt
            };

            return maritialStatusDto;
        }

        // Get all MaritialStatuses and map to DTOs
        public async Task<List<MaritialStatusDTO>> GetAllMaritialStatusesAsync()
        {
            var maritialStatuses = await _context.MaritalStatuses
                .Where(ms => ms.Removed == false)  // Ensure marital statuses are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var maritialStatusDtos = maritialStatuses.Select(ms => new MaritialStatusDTO
            {
                Id = ms.Id,
                Name = ms.Name,
                Removed = ms.Removed,
                CreatedAt = ms.CreatedAt,
                UpdatedAt = ms.UpdatedAt
            }).ToList();

            return maritialStatusDtos;
        }

        // Update a MaritialStatus from DTO
        public async Task<MaritialStatusDTO> UpdateMaritialStatusAsync(int id, MaritialStatusDTO updatedMaritialStatusDto)
        {
            // Validate the incoming DTO data
            if (updatedMaritialStatusDto == null)
                throw new ArgumentNullException(nameof(updatedMaritialStatusDto));

            var existingMaritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (existingMaritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            // Update properties from DTO
            existingMaritialStatus.Name = updatedMaritialStatusDto.Name;
            existingMaritialStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(existingMaritialStatus);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedMaritialStatusDto.Id = existingMaritialStatus.Id;
            updatedMaritialStatusDto.CreatedAt = existingMaritialStatus.CreatedAt;
            updatedMaritialStatusDto.UpdatedAt = existingMaritialStatus.UpdatedAt;

            return updatedMaritialStatusDto;
        }

        // Soft Delete a MaritialStatus by ID
        public async Task<bool> DeleteMaritialStatusAsync(int id)
        {
            var maritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (maritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            maritialStatus.Removed = true;  // Mark as removed
            maritialStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(maritialStatus);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete a MaritialStatus
        public async Task<bool> PermanentlyDeleteMaritialStatusAsync(int id)
        {
            var maritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (maritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            _context.MaritalStatuses.Remove(maritialStatus);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
