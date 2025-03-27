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

        // Create a Designation
        public async Task<Designation> CreateDesignationAsync(Designation designation)
        {
            // Validate incoming data
            if (designation == null)
                throw new ArgumentNullException(nameof(designation));

            designation.CreatedAt = DateTime.UtcNow;
            designation.UpdatedAt = DateTime.UtcNow;

            _context.Designations.Add(designation);
            await _context.SaveChangesAsync();

            return designation;
        }

        // Get a Designation by ID
        public async Task<Designation?> GetDesignationByIdAsync(int id)
        {
            return await _context.Designations
                .FirstOrDefaultAsync(d => d.Id == id && d.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all Designations
        public async Task<List<Designation>> GetAllDesignationsAsync()
        {
            return await _context.Designations
                .Where(d => d.Removed == false)  // Ensure designations are not marked as removed
                .ToListAsync();
        }

        // Update a Designation
        public async Task<Designation> UpdateDesignationAsync(int id, Designation updatedDesignation)
        {
            // Validate the incoming updated designation data
            if (updatedDesignation == null)
                throw new ArgumentNullException(nameof(updatedDesignation));

            var existingDesignation = await _context.Designations.FindAsync(id);

            if (existingDesignation == null)
                throw new KeyNotFoundException("Designation not found.");

            // Update properties
            existingDesignation.Name = updatedDesignation.Name;
            existingDesignation.UpdatedAt = DateTime.UtcNow;

            _context.Designations.Update(existingDesignation);
            await _context.SaveChangesAsync();

            return existingDesignation;
        }

        // Soft Delete a Designation
        public async Task DeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);

            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            designation.Removed = true;  // Mark as removed
            designation.UpdatedAt = DateTime.UtcNow;

            _context.Designations.Update(designation);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a Designation
        public async Task PermanentlyDeleteDesignationAsync(int id)
        {
            var designation = await _context.Designations.FindAsync(id);

            if (designation == null)
                throw new KeyNotFoundException("Designation not found.");

            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();
        }
    }
}
