namespace socialAssistanceFundMIS.Services.MaritalStatusServices
{
    public interface IMaritalStatusService
    {
        Task<MaritalStatusDTO> CreateMaritalStatusAsync(MaritalStatusDTO maritalStatusDto);
        Task<MaritalStatusDTO?> GetMaritalStatusByIdAsync(int id);
        Task<List<MaritalStatusDTO>> GetAllMaritalStatusesAsync();
        Task<MaritalStatusDTO> UpdateMaritalStatusAsync(int id, MaritalStatusDTO updatedMaritalStatusDto);
        Task<bool> DeleteMaritalStatusAsync(int id);
        Task<bool> PermanentlyDeleteMaritalStatusAsync(int id);
    }
}
