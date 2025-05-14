using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services.GeographicLocationServices
{
    public interface IGeographicLocationService
    {
        Task<List<GeographicLocation>> GetVillagesWithHierarchyAsync();
        Task<string> GetVillageHierarchyByIdAsync(int? villageId);
        Task<GeographicLocation> CreateGeographicLocationAsync(GeographicLocation location);
        Task<GeographicLocation?> GetGeographicLocationByIdAsync(int id);
        Task<List<GeographicLocation>> GetAllGeographicLocationsAsync();
        Task<GeographicLocation> UpdateGeographicLocationAsync(int id, GeographicLocation location);
        Task DeleteGeographicLocationAsync(int id);
        Task PermanentlyDeleteGeographicLocationAsync(int id);
    }
}
