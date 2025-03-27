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

        // Create an AssistanceProgram from DTO
        public async Task<AssistanceProgramDTO> CreateAssistanceProgramAsync(AssistanceProgramDTO assistanceProgramDto)
        {
            // Validate incoming DTO data
            if (assistanceProgramDto == null)
                throw new ArgumentNullException(nameof(assistanceProgramDto));

            var assistanceProgram = new AssistanceProgram
            {
                Name = assistanceProgramDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AssistancePrograms.Add(assistanceProgram);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            assistanceProgramDto.Id = assistanceProgram.Id;
            assistanceProgramDto.CreatedAt = assistanceProgram.CreatedAt;
            assistanceProgramDto.UpdatedAt = assistanceProgram.UpdatedAt;

            return assistanceProgramDto;
        }

        // Get an AssistanceProgram by ID and map to DTO
        public async Task<AssistanceProgramDTO?> GetAssistanceProgramByIdAsync(int id)
        {
            var assistanceProgram = await _context.AssistancePrograms
                .FirstOrDefaultAsync(ap => ap.Id == id && ap.Removed == false);  // Ensure it's not marked as removed

            if (assistanceProgram == null)
                return null;

            // Map to DTO
            var assistanceProgramDto = new AssistanceProgramDTO
            {
                Id = assistanceProgram.Id,
                Name = assistanceProgram.Name,
                Removed = assistanceProgram.Removed,
                CreatedAt = assistanceProgram.CreatedAt,
                UpdatedAt = assistanceProgram.UpdatedAt
            };

            return assistanceProgramDto;
        }

        // Get all AssistancePrograms and map to DTOs
        public async Task<List<AssistanceProgramDTO>> GetAllAssistanceProgramsAsync()
        {
            var assistancePrograms = await _context.AssistancePrograms
                .Where(ap => ap.Removed == false)  // Ensure programs are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var assistanceProgramDtos = assistancePrograms.Select(ap => new AssistanceProgramDTO
            {
                Id = ap.Id,
                Name = ap.Name,
                Removed = ap.Removed,
                CreatedAt = ap.CreatedAt,
                UpdatedAt = ap.UpdatedAt
            }).ToList();

            return assistanceProgramDtos;
        }

        // Update an AssistanceProgram from DTO
        public async Task<AssistanceProgramDTO> UpdateAssistanceProgramAsync(int id, AssistanceProgramDTO updatedAssistanceProgramDto)
        {
            // Validate the incoming DTO data
            if (updatedAssistanceProgramDto == null)
                throw new ArgumentNullException(nameof(updatedAssistanceProgramDto));

            var existingProgram = await _context.AssistancePrograms.FindAsync(id);

            if (existingProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            // Update properties from DTO
            existingProgram.Name = updatedAssistanceProgramDto.Name;
            existingProgram.UpdatedAt = DateTime.UtcNow;

            _context.AssistancePrograms.Update(existingProgram);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedAssistanceProgramDto.Id = existingProgram.Id;
            updatedAssistanceProgramDto.CreatedAt = existingProgram.CreatedAt;
            updatedAssistanceProgramDto.UpdatedAt = existingProgram.UpdatedAt;

            return updatedAssistanceProgramDto;
        }

        // Soft Delete an AssistanceProgram by ID
        public async Task<bool> DeleteAssistanceProgramAsync(int id)
        {
            var assistanceProgram = await _context.AssistancePrograms.FindAsync(id);

            if (assistanceProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            assistanceProgram.Removed = true;  // Mark as removed
            assistanceProgram.UpdatedAt = DateTime.UtcNow;

            _context.AssistancePrograms.Update(assistanceProgram);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete an AssistanceProgram
        public async Task<bool> PermanentlyDeleteAssistanceProgramAsync(int id)
        {
            var assistanceProgram = await _context.AssistancePrograms.FindAsync(id);

            if (assistanceProgram == null)
                throw new KeyNotFoundException("AssistanceProgram not found.");

            _context.AssistancePrograms.Remove(assistanceProgram);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
