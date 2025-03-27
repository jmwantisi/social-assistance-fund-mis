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

        // Create a MaritialStatus
        public async Task<MaritialStatus> CreateMaritialStatusAsync(MaritialStatus maritialStatus)
        {
            // Validate incoming data
            if (maritialStatus == null)
                throw new ArgumentNullException(nameof(maritialStatus));

            maritialStatus.CreatedAt = DateTime.UtcNow;
            maritialStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Add(maritialStatus);
            await _context.SaveChangesAsync();

            return maritialStatus;
        }

        // Get a MaritialStatus by ID
        public async Task<MaritialStatus?> GetMaritialStatusByIdAsync(int id)
        {
            return await _context.MaritalStatuses
                .FirstOrDefaultAsync(ms => ms.Id == id && ms.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all MaritialStatuses
        public async Task<List<MaritialStatus>> GetAllMaritialStatusesAsync()
        {
            return await _context.MaritalStatuses
                .Where(ms => ms.Removed == false)  // Ensure marital statuses are not marked as removed
                .ToListAsync();
        }

        // Update a MaritialStatus
        public async Task<MaritialStatus> UpdateMaritialStatusAsync(int id, MaritialStatus updatedMaritialStatus)
        {
            // Validate the incoming updated marital status data
            if (updatedMaritialStatus == null)
                throw new ArgumentNullException(nameof(updatedMaritialStatus));

            var existingMaritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (existingMaritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            // Update properties
            existingMaritialStatus.Name = updatedMaritialStatus.Name;
            existingMaritialStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(existingMaritialStatus);
            await _context.SaveChangesAsync();

            return existingMaritialStatus;
        }

        // Soft Delete a MaritialStatus
        public async Task DeleteMaritialStatusAsync(int id)
        {
            var maritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (maritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            maritialStatus.Removed = true;  // Mark as removed
            maritialStatus.UpdatedAt = DateTime.UtcNow;

            _context.MaritalStatuses.Update(maritialStatus);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a MaritialStatus
        public async Task PermanentlyDeleteMaritialStatusAsync(int id)
        {
            var maritialStatus = await _context.MaritalStatuses.FindAsync(id);

            if (maritialStatus == null)
                throw new KeyNotFoundException("Marital Status not found.");

            _context.MaritalStatuses.Remove(maritialStatus);
            await _context.SaveChangesAsync();
        }
    }
}
