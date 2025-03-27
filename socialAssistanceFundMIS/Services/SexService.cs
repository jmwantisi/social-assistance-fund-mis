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

        // Create a Sex
        public async Task<Sex> CreateSexAsync(Sex sex)
        {
            // Validate incoming data
            if (sex == null)
                throw new ArgumentNullException(nameof(sex));

            sex.CreatedAt = DateTime.UtcNow;
            sex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Add(sex);
            await _context.SaveChangesAsync();

            return sex;
        }

        // Get a Sex by ID
        public async Task<Sex?> GetSexByIdAsync(int id)
        {
            return await _context.Sexes
                .FirstOrDefaultAsync(s => s.Id == id && s.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all Sexes
        public async Task<List<Sex>> GetAllSexesAsync()
        {
            return await _context.Sexes
                .Where(s => s.Removed == false)  // Ensure sexes are not marked as removed
                .ToListAsync();
        }

        // Update a Sex
        public async Task<Sex> UpdateSexAsync(int id, Sex updatedSex)
        {
            // Validate the incoming updated sex data
            if (updatedSex == null)
                throw new ArgumentNullException(nameof(updatedSex));

            var existingSex = await _context.Sexes.FindAsync(id);

            if (existingSex == null)
                throw new KeyNotFoundException("Sex not found.");

            // Update properties
            existingSex.Name = updatedSex.Name;
            existingSex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(existingSex);
            await _context.SaveChangesAsync();

            return existingSex;
        }

        // Soft Delete a Sex
        public async Task DeleteSexAsync(int id)
        {
            var sex = await _context.Sexes.FindAsync(id);

            if (sex == null)
                throw new KeyNotFoundException("Sex not found.");

            sex.Removed = true;  // Mark as removed
            sex.UpdatedAt = DateTime.UtcNow;

            _context.Sexes.Update(sex);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a Sex
        public async Task PermanentlyDeleteSexAsync(int id)
        {
            var sex = await _context.Sexes.FindAsync(id);

            if (sex == null)
                throw new KeyNotFoundException("Sex not found.");

            _context.Sexes.Remove(sex);
            await _context.SaveChangesAsync();
        }
    }
}
