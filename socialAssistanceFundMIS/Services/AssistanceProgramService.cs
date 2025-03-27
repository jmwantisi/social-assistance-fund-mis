using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class AssistanceProgramService
    {
        private readonly ApplicationDbContext _context;

        public AssistanceProgramService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create an AssistanceProgram
        public async Task<AssistanceProgram> CreateAssistanceProgramAsync(AssistanceProgram assistanceProgram)
        {
            // Validate incoming data
            if (assistanceProgram == null)
                throw new ArgumentNullException(nameof(assistanceProgram));

            assistanceProgram.CreatedAt = DateTime.UtcNow;
            assistanceProgram.UpdatedAt = DateTime.UtcNow;

            _context.AssistancePrograms.Add(assistanceProgram);
            await _context.SaveChangesAsync();

            return assistanceProgram;
        }

        // Get an AssistanceProgram by ID
        public async Task<AssistanceProgram?> GetAssistanceProgramByIdAsync(int id)
        {
            return await _context.AssistancePrograms
                .FirstOrDefaultAsync(ap => ap.Id == id && ap.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all AssistancePrograms
        public async Task<List<AssistanceProgram>> GetAllAssistanceProgramsAsync()
        {
            return await _context.AssistancePrograms
                .Where(ap => ap.Removed == false)  // Ensure programs are not marked as removed
                .ToListAsync();
        }

        // Update an AssistanceProgram
        public async Task<AssistanceProgram> UpdateAssistanceProgramAsync(int id, AssistanceProgram updatedAssistanceProgram)
        {
            // Validate the incoming updated program data
            if (updatedAssistanceProgram == null)
                throw new ArgumentNullException(nameof(updatedAssistanceProgram));

            var existingProgram = await _context.AssistancePrograms.FindAsync(id);

            if (existingProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            // Update properties
            existingProgram.Name = updatedAssistanceProgram.Name;
            existingProgram.UpdatedAt = DateTime.UtcNow;

            _context.AssistancePrograms.Update(existingProgram);
            await _context.SaveChangesAsync();

            return existingProgram;
        }

        // Soft Delete an AssistanceProgram
        public async Task DeleteAssistanceProgramAsync(int id)
        {
            var assistanceProgram = await _context.AssistancePrograms.FindAsync(id);

            if (assistanceProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            assistanceProgram.Removed = true;  // Mark as removed
            assistanceProgram.UpdatedAt = DateTime.UtcNow;

            _context.AssistancePrograms.Update(assistanceProgram);
            await _context.SaveChangesAsync();
        }

        // Permanently delete an AssistanceProgram
        public async Task PermanentlyDeleteAssistanceProgramAsync(int id)
        {
            var assistanceProgram = await _context.AssistancePrograms.FindAsync(id);

            if (assistanceProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            _context.AssistancePrograms.Remove(assistanceProgram);
            await _context.SaveChangesAsync();
        }
    }
}
