using socialAssistanceFundMIS.Data;

namespace socialAssistanceFundMIS.Services
{
    public interface IApplicantService
    {
        Task<List<Applicant>> GetAllApplicantsAsync();
        Task<Applicant?> GetApplicantByIdAsync(int id);
        Task<bool> AddApplicantAsync(Applicant applicant);
        Task<bool> UpdateApplicantAsync(Applicant applicant);
        Task<bool> DeleteApplicantAsync(int id);
    }
}
