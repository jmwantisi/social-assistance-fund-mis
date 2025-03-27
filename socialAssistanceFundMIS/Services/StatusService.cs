using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
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
        public async Task<Status> CreateStatusAsync(Status status)
        {
            // Validate incoming data
            if (status == null)
                throw new ArgumentNullException(nameof(status));

            status.CreatedAt = DateTime.UtcNow;
            status.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

        // Get a Status by ID
        public async Task<Status?> GetStatusByIdAsync(int id)
        {
            return await _context.Statuses
                .FirstOrDefaultAsync(s => s.Id == id && s.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all Statuses
        public async Task<List<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses
                .Where(s => s.Removed == false)  // Ensure statuses are not marked as removed
                .ToListAsync();
        }

        // Update a Status
        public async Task<Status> UpdateStatusAsync(int id, Status updatedStatus)
        {
            // Validate the incoming updated status data
            if (updatedStatus == null)
                throw new ArgumentNullException(nameof(updatedStatus));

            var existingStatus = await _context.Statuses.FindAsync(id);

            if (existingStatus == null)
                throw new KeyNotFoundException("Status not found.");

            // Update properties
            existingStatus.Name = updatedStatus.Name;
            existingStatus.UpdatedAt = DateTime.UtcNow;

            _context.Statuses.Update(existingStatus);
            await _context.SaveChangesAsync();

            return existingStatus;
        }

        // Soft Delete a Status
        public async Task DeleteStatusAsync(int id)
        {
            var status = await _context.Statuses.FindAsync(id);

            if (status == null)
                throw new KeyNotFoundException("Status not found.");

            status.Removed = true;  // Mark as removed
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
