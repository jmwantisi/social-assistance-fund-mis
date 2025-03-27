using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.DTOs;
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
        public async Task<GeographicLocationDTO> CreateGeographicLocationAsync(GeographicLocationDTO geographicLocationDTO)
        {
            if (geographicLocationDTO == null)
                throw new ArgumentNullException(nameof(geographicLocationDTO));

            var geographicLocation = new GeographicLocation
            {
                Name = geographicLocationDTO.Name,
                GeographicLocationTypeId = geographicLocationDTO.GeographicLocationTypeId,
                GeographicLocationParentId = geographicLocationDTO.GeographicLocationParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.GeographicLocations.Add(geographicLocation);
            await _context.SaveChangesAsync();

            // Map to GeographicLocationDTO
            return new GeographicLocationDTO
            {
                Id = geographicLocation.Id,
                Name = geographicLocation.Name,
                GeographicLocationTypeId = geographicLocation.GeographicLocationTypeId,
                GeographicLocationParentId = geographicLocation.GeographicLocationParentId,
                Removed = geographicLocation.Removed,
                CreatedAt = geographicLocation.CreatedAt,
                UpdatedAt = geographicLocation.UpdatedAt
            };
        }

        // Get a GeographicLocation by ID
        public async Task<GeographicLocationDTO?> GetGeographicLocationByIdAsync(int id)
        {
            var geographicLocation = await _context.GeographicLocations
                .Include(gl => gl.GeographicLocationType) // Include related GeographicLocationType data
                .Include(gl => gl.ParentLocation) // Include related ParentLocation data
                .FirstOrDefaultAsync(gl => gl.Id == id && gl.Removed == false);

            if (geographicLocation == null)
                return null;

            // Map to GeographicLocationDTO
            return new GeographicLocationDTO
            {
                Id = geographicLocation.Id,
                Name = geographicLocation.Name,
                GeographicLocationTypeId = geographicLocation.GeographicLocationTypeId,
                GeographicLocationParentId = geographicLocation.GeographicLocationParentId,
                Removed = geographicLocation.Removed,
                CreatedAt = geographicLocation.CreatedAt,
                UpdatedAt = geographicLocation.UpdatedAt,
                GeographicLocationType = geographicLocation.GeographicLocationType != null ?
                    new GeographicLocationTypeDTO
                    {
                        Id = geographicLocation.GeographicLocationType.Id,
                        Name = geographicLocation.GeographicLocationType.Name
                    } : null,
                ParentLocation = geographicLocation.ParentLocation != null ?
                    new GeographicLocationDTO
                    {
                        Id = geographicLocation.ParentLocation.Id,
                        Name = geographicLocation.ParentLocation.Name
                    } : null
            };
        }

        // Get all GeographicLocations
        public async Task<List<GeographicLocationDTO>> GetAllGeographicLocationsAsync()
        {
            var geographicLocations = await _context.GeographicLocations
                .Include(gl => gl.GeographicLocationType)
                .Include(gl => gl.ParentLocation)
                .Where(gl => gl.Removed == false)
                .ToListAsync();

            return geographicLocations.Select(gl => new GeographicLocationDTO
            {
                Id = gl.Id,
                Name = gl.Name,
                GeographicLocationTypeId = gl.GeographicLocationTypeId,
                GeographicLocationParentId = gl.GeographicLocationParentId,
                Removed = gl.Removed,
                CreatedAt = gl.CreatedAt,
                UpdatedAt = gl.UpdatedAt,
                GeographicLocationType = gl.GeographicLocationType != null ?
                    new GeographicLocationTypeDTO
                    {
                        Id = gl.GeographicLocationType.Id,
                        Name = gl.GeographicLocationType.Name
                    } : null,
                ParentLocation = gl.ParentLocation != null ?
                    new GeographicLocationDTO
                    {
                        Id = gl.ParentLocation.Id,
                        Name = gl.ParentLocation.Name
                    } : null
            }).ToList();
        }

        // Update a GeographicLocation
        public async Task<GeographicLocationDTO> UpdateGeographicLocationAsync(int id, GeographicLocationDTO updatedGeographicLocationDTO)
        {
            if (updatedGeographicLocationDTO == null)
                throw new ArgumentNullException(nameof(updatedGeographicLocationDTO));

            var existingGeographicLocation = await _context.GeographicLocations.FindAsync(id);

            if (existingGeographicLocation == null)
                throw new KeyNotFoundException("Geographic Location not found.");

            existingGeographicLocation.Name = updatedGeographicLocationDTO.Name;
            existingGeographicLocation.GeographicLocationTypeId = updatedGeographicLocationDTO.GeographicLocationTypeId;
            existingGeographicLocation.GeographicLocationParentId = updatedGeographicLocationDTO.GeographicLocationParentId;
            existingGeographicLocation.UpdatedAt = DateTime.UtcNow;

            _context.GeographicLocations.Update(existingGeographicLocation);
            await _context.SaveChangesAsync();

            // Map to GeographicLocationDTO
            return new GeographicLocationDTO
            {
                Id = existingGeographicLocation.Id,
                Name = existingGeographicLocation.Name,
                GeographicLocationTypeId = existingGeographicLocation.GeographicLocationTypeId,
                GeographicLocationParentId = existingGeographicLocation.GeographicLocationParentId,
                Removed = existingGeographicLocation.Removed,
                CreatedAt = existingGeographicLocation.CreatedAt,
                UpdatedAt = existingGeographicLocation.UpdatedAt
            };
        }

        // Soft Delete a GeographicLocation
        public async Task DeleteGeographicLocationAsync(int id)
        {
            var geographicLocation = await _context.GeographicLocations.FindAsync(id);

            if (geographicLocation == null)
                throw new KeyNotFoundException("Geographic Location not found.");

            geographicLocation.Removed = true;
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
