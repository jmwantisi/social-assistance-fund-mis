using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class StatusService
    {
        private readonly ApplicationDbContext _context;

        public StatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Status
        public async Task<StatusDTO> CreateStatusAsync(StatusDTO statusDTO)
        {
            if (statusDTO == null)
                throw new ArgumentNullException(nameof(statusDTO));

            var status = new Status
            {
                Name = statusDTO.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            // Return the StatusDTO with populated ID after saving
            return new StatusDTO
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        // Get a Status by ID
        public async Task<StatusDTO?> GetStatusByIdAsync(int id)
        {
            var status = await _context.Statuses
                .FirstOrDefaultAsync(s => s.Id == id && s.Removed == false);

            if (status == null)
                return null;

            // Map the Status entity to StatusDTO
            return new StatusDTO
            {
                Id = status.Id,
                Name = status.Name
            };
        }

        // Get all Statuses
        public async Task<List<StatusDTO>> GetAllStatusesAsync()
        {
            var statuses = await _context.Statuses
                .Where(s => s.Removed == false)
                .ToListAsync();

            // Map the Status entities to StatusDTOs
            return statuses.Select(s => new StatusDTO
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
        }

        // Update a Status
        public async Task<StatusDTO> UpdateStatusAsync(int id, StatusDTO updatedStatusDTO)
        {
            if (updatedStatusDTO == null)
                throw new ArgumentNullException(nameof(updatedStatusDTO));

            var existingStatus = await _context.Statuses.FindAsync(id);

            if (existingStatus == null)
                throw new KeyNotFoundException("Status not found.");

            // Update the properties of the existing Status
            existingStatus.Name = updatedStatusDTO.Name;
            existingStatus.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(existingStatus);
            await _context.SaveChangesAsync();

            // Map and return the updated StatusDTO
            return new StatusDTO
            {
                Id = existingStatus.Id,
                Name = existingStatus.Name
            };
        }

        // Soft Delete a Status
        public async Task DeleteStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
                throw new KeyNotFoundException("Status not found.");

            status.Removed = true; // Soft delete (mark as removed)
            status.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a Status
        public async Task PermanentlyDeleteStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
                throw new KeyNotFoundException("Status not found.");

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();
        }
    }
}
