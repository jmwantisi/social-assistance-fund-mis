using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class OfficialRecordService
    {
        private readonly ApplicationDbContext _context;

        public OfficialRecordService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create an OfficialRecord
        public async Task<OfficialRecord> CreateOfficialRecordAsync(OfficialRecord officialRecord)
        {
            // Validate incoming data
            if (officialRecord == null)
                throw new ArgumentNullException(nameof(officialRecord));

            officialRecord.CreatedAt = DateTime.UtcNow;
            officialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Add(officialRecord);
            await _context.SaveChangesAsync();

            return officialRecord;
        }

        // Get an OfficialRecord by ID
        public async Task<OfficialRecord?> GetOfficialRecordByIdAsync(int id)
        {
            return await _context.OfficialRecords
                .Include(o => o.Officer)  // Include related Officer data
                .FirstOrDefaultAsync(o => o.Id == id && o.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all OfficialRecords
        public async Task<List<OfficialRecord>> GetAllOfficialRecordsAsync()
        {
            return await _context.OfficialRecords
                .Include(o => o.Officer)
                .Where(o => o.Removed == false)  // Ensure records are not marked as removed
                .ToListAsync();
        }

        // Update an OfficialRecord
        public async Task<OfficialRecord> UpdateOfficialRecordAsync(int id, OfficialRecord updatedOfficialRecord)
        {
            // Validate the incoming updated official record data
            if (updatedOfficialRecord == null)
                throw new ArgumentNullException(nameof(updatedOfficialRecord));

            var existingOfficialRecord = await _context.OfficialRecords.FindAsync(id);

            if (existingOfficialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            // Update properties
            existingOfficialRecord.OfficerId = updatedOfficialRecord.OfficerId;
            existingOfficialRecord.OfficiationDate = updatedOfficialRecord.OfficiationDate;
            existingOfficialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(existingOfficialRecord);
            await _context.SaveChangesAsync();

            return existingOfficialRecord;
        }

        // Soft Delete an OfficialRecord
        public async Task DeleteOfficialRecordAsync(int id)
        {
            var officialRecord = await _context.OfficialRecords.FindAsync(id);

            if (officialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            officialRecord.Removed = true;  // Mark as removed
            officialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(officialRecord);
            await _context.SaveChangesAsync();
        }

        // Permanently delete an OfficialRecord
        public async Task PermanentlyDeleteOfficialRecordAsync(int id)
        {
            var officialRecord = await _context.OfficialRecords.FindAsync(id);

            if (officialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            _context.OfficialRecords.Remove(officialRecord);
            await _context.SaveChangesAsync();
        }
    }
}
