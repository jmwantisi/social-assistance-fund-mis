using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class DesignationService
    {
        private readonly ApplicationDbContext _context;

        public DesignationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a Designation from DTO
        public async Task<DesignationDTO> CreateDesignationAsync(DesignationDTO designationDto)
        {
            // Validate incoming DTO data
            if (designationDto == null)
                throw new ArgumentNullException(nameof(designationDto));

            var designation = new Designation
            {
                Name = designationDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Designations.Add(designation);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            designationDto.Id = designation.Id;
            designationDto.CreatedAt = designation.CreatedAt;
            designationDto.UpdatedAt = designation.UpdatedAt;

            return designationDto;
        }

        // Get a Designation by ID and map to DTO
        public async Task<DesignationDTO?> GetDesignationByIdAsync(int id)
        {
            var designation = await _context.Designations
                .FirstOrDefaultAsync(d => d.Id == id && d.Removed == false);  // Ensure it's not marked as removed

            if (designation == null)
                return null;

            // Map to DTO
            var designationDto = new DesignationDTO
            {
                Id = designation.Id,
                Name = designation.Name,
                Removed = designation.Removed,
                CreatedAt = designation.CreatedAt,
                UpdatedAt = designation.UpdatedAt
            };

            return designationDto;
        }

        // Get all Designations and map to DTOs
        public async Task<List<DesignationDTO>> GetAllDesignationsAsync()
        {
            var designations = await _context.Designations
                .Where(d => d.Removed == false)  // Ensure designations are not marked as removed
                .ToListAsync();

            // Map to DTOs
            var designationDtos = designations.Select(d => new DesignationDTO
            {
                Id = d.Id,
                Name = d.Name,
                Removed = d.Removed,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList();

            return designationDtos;
        }

        // Update a Designation from DTO
        public async Task<DesignationDTO> UpdateDesignationAsync(int id, DesignationDTO updatedDesignationDto)
        {
            // Validate the incoming DTO data
            if (updatedDesignationDto == null)
                throw new ArgumentNullException(nameof(updatedDesignationDto));

            var existingDesignation = await _context.Designations.FindAsync(id);

            if (existingDesignation == null)
                throw new KeyNotFoundException("Designation not found.");

            // Update properties from DTO
            existingDesignation.Name = updatedDesignationDto.Name;
            existingDesignation.UpdatedAt = DateTime.UtcNow;

            _context.Designations.Update(existingDesignation);
            await _context.SaveChangesAsync();

            // Map back to DTO for return
            updatedDesignationDto.Id = existingDesignation.Id;
            updatedDesignationDto.CreatedAt = existingDesignation.CreatedAt;
            updatedDesignationDto.UpdatedAt = existingDesignation.UpdatedAt;

            return updatedDesignationDto;
        }

        // Soft Delete a Designation by ID
        public async Task<bool> DeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);

            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            designation.Removed = true;  // Mark as removed
            designation.UpdatedAt = DateTime.UtcNow;

            _context.Designations.Update(designation);
            await _context.SaveChangesAsync();

            return true;
        }

        // Permanently delete a Designation
        public async Task<bool> PermanentlyDeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);

            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
