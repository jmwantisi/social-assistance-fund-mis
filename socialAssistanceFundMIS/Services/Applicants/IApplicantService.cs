using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Services.Applicants
{
    public interface IApplicantService
    {
        Task<ApplicantViewModel> CreateApplicantAsync(Applicant applicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers);
        Task<ApplicantViewModel> GetApplicantByIdAsync(int id);
        Task<List<ApplicantViewModel>> GetAllApplicantsAsync();
        Task<ApplicantViewModel> UpdateApplicantAsync(int id, Applicant applicant, List<string> phoneNumbers);
        Task<bool> DeleteApplicantAsync(int id);
        Task<bool> DeletePhoneNumberAsync(int applicantId, int phoneNumberId);
    }

}
