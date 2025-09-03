using socialAssistanceFundMIS.Common;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Services.Applicants
{
    public interface IApplicantService
    {
        Task<Result<ApplicantViewModel>> CreateApplicantAsync(Applicant applicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers);
        Task<Result<ApplicantViewModel>> GetApplicantByIdAsync(int id);
        Task<Result<List<ApplicantViewModel>>> GetAllApplicantsAsync();
        Task<Result<ApplicantViewModel>> UpdateApplicantAsync(int id, Applicant updatedApplicant, List<(string phoneNumber, int phoneNumberTypeId)> phoneNumbers);
        Task<Result> DeleteApplicantAsync(int id);
        Task<Result> DeletePhoneNumberAsync(int applicantId, int phoneNumberId);
        Task<Result<List<ApplicantViewModel>>> SearchApplicantsAsync(string searchTerm, int page = 1, int pageSize = 20);
        Task<Result<bool>> IsIdentityCardNumberUniqueAsync(string identityCardNumber, int? excludeId = null);
        Task<Result<bool>> IsEmailUniqueAsync(string email, int? excludeId = null);
    }
}
