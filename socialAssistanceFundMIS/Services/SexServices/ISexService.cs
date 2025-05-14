namespace socialAssistanceFundMIS.Services.SexServices
{
    public interface ISexService
    {
        Task<SexDTO> CreateSexAsync(SexDTO sexDto);
        Task<SexDTO?> GetSexByIdAsync(int id);
        Task<List<SexDTO>> GetAllSexesAsync();
        Task<SexDTO> UpdateSexAsync(int id, SexDTO updatedSexDto);
        Task<bool> DeleteSexAsync(int id);
        Task<bool> PermanentlyDeleteSexAsync(int id);
    }
}
