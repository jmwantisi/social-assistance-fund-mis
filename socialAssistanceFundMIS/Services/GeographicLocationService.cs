using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using Microsoft.EntityFrameworkCore;

namespace socialAssistanceFundMIS.Services
{
    public class GeographicLocationService
    {
        private readonly ApplicationDbContext _context;

        public GeographicLocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a GeographicLocation
        public async Task<GeographicLocation> CreateGeographicLocationAsync(GeographicLocation geographicLocation)
        {
            // Validate incoming data
            if (geographicLocation == null)
                throw new ArgumentNullException(nameof(geographicLocation));

            geographicLocation.CreatedAt = DateTime.UtcNow;
            geographicLocation.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Add(geographicLocation);
            await _context.SaveChangesAsync();

            return geographicLocation;
        }

        // Get a GeographicLocation by ID
        public async Task<GeographicLocation?> GetGeographicLocationByIdAsync(int id)
        {
            return await _context.GeographicLocations
                .Include(gl => gl.GeographicLocationType)  // Include related data like GeographicLocationType
                .Include(gl => gl.ParentLocation)          // Include related data like ParentLocation
                .FirstOrDefaultAsync(gl => gl.Id == id && gl.Removed == false);  // Ensure it's not marked as removed
        }

        // Get all GeographicLocations
        public async Task<List<GeographicLocation>> GetAllGeographicLocationsAsync()
        {
            return await _context.GeographicLocations
                .Include(gl => gl.GeographicLocationType)
                .Include(gl => gl.ParentLocation)
                .Where(gl => gl.Removed == false)  // Ensure geographic locations are not marked as removed
                .ToListAsync();
        }

        // Update a GeographicLocation
        public async Task<GeographicLocation> UpdateGeographicLocationAsync(int id, GeographicLocation updatedGeographicLocation)
        {
            // Validate the incoming updated geographic location data
            if (updatedGeographicLocation == null)
                throw new ArgumentNullException(nameof(updatedGeographicLocation));

            var existingGeographicLocation = await _context.GeographicLocations.FindAsync(id);

            if (existingGeographicLocation == null)
                throw new KeyNotFoundException("Geographic Location not found.");

            // Update properties
            existingGeographicLocation.Name = updatedGeographicLocation.Name;
            existingGeographicLocation.GeographicLocationTypeId = updatedGeographicLocation.GeographicLocationTypeId;
            existingGeographicLocation.GeographicLocationParentId = updatedGeographicLocation.GeographicLocationParentId;
            existingGeographicLocation.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Update(existingGeographicLocation);
            await _context.SaveChangesAsync();

            return existingGeographicLocation;
        }

        // Soft Delete a GeographicLocation
        public async Task DeleteGeographicLocationAsync(int id)
        {
            var geographicLocation = await _context.GeographicLocations.FindAsync(id);

            if (geographicLocation == null)
                throw new KeyNotFoundException("Geographic Location not found.");

            geographicLocation.Removed = true;  // Mark as removed
            geographicLocation.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Update(geographicLocation);
            await _context.SaveChangesAsync();
        }

        // Permanently delete a GeographicLocation
        public async Task PermanentlyDeleteGeographicLocationAsync(int id)
        {
            var geographicLocation = await _context.GeographicLocations.FindAsync(id);

            if (geographicLocation == null)
                throw new KeyNotFoundException("Geographic Location not found.");

            _context.GeographicLocations.Remove(geographicLocation);
            await _context.SaveChangesAsync();
        }
    }
}
