using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class SexService
    {
        private readonly ApplicationDbContext _context;

        public SexService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Sex from DTO
        public async Task<SexDTO> CreateSexAsync(SexDTO sexDto)
        {
            // Validate incoming DTO data
            if (sexDto == null)
                throw new ArgumentNullException(nameof(sexDto));

            var sex = new Sex
            {
                Name = sexDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Sexes.Add(sex);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            sexDto.Id = sex.Id;
            sexDto.CreatedAt = sex.CreatedAt;
            sexDto.UpdatedAt = sex.UpdatedAt;

            return sexDto;
        }

        // Get a Sex by ID and map to DTO
        public async Task<SexDTO?> GetSexByIdAsync(int id)
        {
            var sex = await _context.Sexes
                .FirstOrDefaultAsync(s => s.Id == id && s.Removed == false);  // Ensure it's not marked as removed

            if (sex == null)
                return null;

            // Map to DTO
            var sexDto = new SexDTO
            {
                Id = sex.Id,
                Name = sex.Name,
                Removed = sex.Removed,
                CreatedAt = sex.CreatedAt,
                UpdatedAt = sex.UpdatedAt
            };

            return sexDto;
        }

        // Get all Sexes and map to DTOs
        public async Task<List<SexDTO>> GetAllSexesAsync()
        {
            var sexes = await _context.Sexes
                .Where(s => s.Removed == false)  // Ensure sexes are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var sexDtos = sexes.Select(s => new SexDTO
            {
                Id = s.Id,
                Name = s.Name,
                Removed = s.Removed,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            }).ToList();

            return sexDtos;
        }

        // Update a Sex from DTO
        public async Task<SexDTO> UpdateSexAsync(int id, SexDTO updatedSexDto)
        {
            // Validate the incoming DTO data
            if (updatedSexDto == null)
                throw new ArgumentNullException(nameof(updatedSexDto));

            var existingSex = await _context.Sexes.FindAsync(id);

            if (existingSex == null)
                throw new KeyNotFoundException("Sex not found.");

            // Update properties from DTO
            existingSex.Name = updatedSexDto.Name;
            existingSex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(existingSex);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedSexDto.Id = existingSex.Id;
            updatedSexDto.CreatedAt = existingSex.CreatedAt;
            updatedSexDto.UpdatedAt = existingSex.UpdatedAt;

            return updatedSexDto;
        }

        // Soft Delete a Sex by ID
        public async Task<bool> DeleteSexAsync(int id)
        {
            var sex = await _context.Sexes.FindAsync(id);

            if (sex == null)
                throw new KeyNotFoundException("Sex not found.");

            sex.Removed = true;  // Mark as removed
            sex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(sex);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete a Sex
        public async Task<bool> PermanentlyDeleteSexAsync(int id)
        {
            var sex = await _context.Sexes.FindAsync(id);

            if (sex == null)
                throw new KeyNotFoundException("Sex not found.");

            _context.Sexes.Remove(sex);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
