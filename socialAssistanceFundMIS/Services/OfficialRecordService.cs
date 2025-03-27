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

        // Create an OfficialRecord from DTO
        public async Task<OfficialRecordDTO> CreateOfficialRecordAsync(OfficialRecordDTO officialRecordDto)
        {
            // Validate incoming DTO data
            if (officialRecordDto == null)
                throw new ArgumentNullException(nameof(officialRecordDto));

            var officialRecord = new OfficialRecord
            {
                OfficerId = officialRecordDto.OfficerId,
                OfficiationDate = officialRecordDto.OfficiationDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.OfficialRecords.Add(officialRecord);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            officialRecordDto.Id = officialRecord.Id;
            officialRecordDto.CreatedAt = officialRecord.CreatedAt;
            officialRecordDto.UpdatedAt = officialRecord.UpdatedAt;

            return officialRecordDto;
        }

        // Get an OfficialRecord by ID and map to DTO
        public async Task<OfficialRecordDTO?> GetOfficialRecordByIdAsync(int id)
        {
            var officialRecord = await _context.OfficialRecords
                .Include(o => o.Officer)  // Include related Officer data
                .FirstOrDefaultAsync(o => o.Id == id && o.Removed == false);  // Ensure it's not marked as removed

            if (officialRecord == null)
                return null;

            // Map to DTO
            var officialRecordDto = new OfficialRecordDTO
            {
                Id = officialRecord.Id,
                OfficerId = officialRecord.OfficerId,
                OfficiationDate = officialRecord.OfficiationDate,
                Removed = officialRecord.Removed,
                CreatedAt = officialRecord.CreatedAt,
                UpdatedAt = officialRecord.UpdatedAt
            };

            return officialRecordDto;
        }

        // Get all OfficialRecords and map to DTOs
        public async Task<List<OfficialRecordDTO>> GetAllOfficialRecordsAsync()
        {
            var officialRecords = await _context.OfficialRecords
                .Include(o => o.Officer)
                .Where(o => o.Removed == false)  // Ensure records are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var officialRecordDtos = officialRecords.Select(o => new OfficialRecordDTO
            {
                Id = o.Id,
                OfficerId = o.OfficerId,
                OfficiationDate = o.OfficiationDate,
                Removed = o.Removed,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            }).ToList();

            return officialRecordDtos;
        }

        // Update an OfficialRecord from DTO
        public async Task<OfficialRecordDTO> UpdateOfficialRecordAsync(int id, OfficialRecordDTO updatedOfficialRecordDto)
        {
            // Validate the incoming DTO data
            if (updatedOfficialRecordDto == null)
                throw new ArgumentNullException(nameof(updatedOfficialRecordDto));

            var existingOfficialRecord = await _context.OfficialRecords.FindAsync(id);

            if (existingOfficialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            // Update properties from DTO
            existingOfficialRecord.OfficerId = updatedOfficialRecordDto.OfficerId;
            existingOfficialRecord.OfficiationDate = updatedOfficialRecordDto.OfficiationDate;
            existingOfficialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(existingOfficialRecord);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedOfficialRecordDto.Id = existingOfficialRecord.Id;
            updatedOfficialRecordDto.CreatedAt = existingOfficialRecord.CreatedAt;
            updatedOfficialRecordDto.UpdatedAt = existingOfficialRecord.UpdatedAt;

            return updatedOfficialRecordDto;
        }

        // Soft Delete an OfficialRecord by ID
        public async Task<bool> DeleteOfficialRecordAsync(int id)
        {
            var officialRecord = await _context.OfficialRecords.FindAsync(id);

            if (officialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            officialRecord.Removed = true;  // Mark as removed
            officialRecord.UpdatedAt = DateTime.UtcNow;

            _context.OfficialRecords.Update(officialRecord);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete an OfficialRecord
        public async Task<bool> PermanentlyDeleteOfficialRecordAsync(int id)
        {
            var officialRecord = await _context.OfficialRecords.FindAsync(id);

            if (officialRecord == null)
                throw new KeyNotFoundException("OfficialRecord not found.");

            _context.OfficialRecords.Remove(officialRecord);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
