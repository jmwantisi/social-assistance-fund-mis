using socialAssistanceFundMIS.DTOs;

namespace socialAssistanceFundMIS.Services.StatusServices
{
    public interface IStatusService
    {
        Task<StatusDTO> CreateStatusAsync(StatusDTO statusDTO);
        Task<StatusDTO?> GetStatusByIdAsync(int id);
        Task<List<StatusDTO>> GetAllStatusesAsync();
        Task<StatusDTO> UpdateStatusAsync(int id, StatusDTO updatedStatusDTO);
        Task DeleteStatusAsync(int id);
        Task PermanentlyDeleteStatusAsync(int id);
    }
}
