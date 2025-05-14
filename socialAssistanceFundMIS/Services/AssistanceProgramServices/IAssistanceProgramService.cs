namespace socialAssistanceFundMIS.Services.AssistanceProgramServices
{
    public interface IAssistanceProgramService
    {
        Task<AssistanceProgramDTO> CreateAssistanceProgramAsync(AssistanceProgramDTO dto);
        Task<AssistanceProgramDTO?> GetAssistanceProgramByIdAsync(int id);
        Task<List<AssistanceProgramDTO>> GetAllAssistanceProgramsAsync();
        Task<AssistanceProgramDTO> UpdateAssistanceProgramAsync(int id, AssistanceProgramDTO dto);
        Task<bool> DeleteAssistanceProgramAsync(int id);
        Task<bool> PermanentlyDeleteAssistanceProgramAsync(int id);
    }
}
