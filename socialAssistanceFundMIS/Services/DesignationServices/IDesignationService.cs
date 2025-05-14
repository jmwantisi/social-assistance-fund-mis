namespace socialAssistanceFundMIS.Services.DesignationServices
{
    public interface IDesignationService
    {
        Task<DesignationDTO> CreateDesignationAsync(DesignationDTO designationDto);
        Task<DesignationDTO?> GetDesignationByIdAsync(int id);
        Task<List<DesignationDTO>> GetAllDesignationsAsync();
        Task<DesignationDTO> UpdateDesignationAsync(int id, DesignationDTO updatedDesignationDto);
        Task<bool> DeleteDesignationAsync(int id);
        Task<bool> PermanentlyDeleteDesignationAsync(int id);
    }
}
