using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services.OfficerServices
{
    public interface IOfficerService
    {
        Task<IEnumerable<Officer>> GetAllAsync();
        Task<Officer?> GetByIdAsync(int id);
        Task<Officer> CreateAsync(Officer officer);
        Task<Officer> UpdateAsync(int id, Officer officer);
        Task<bool> DeleteAsync(int id);
    }
}
