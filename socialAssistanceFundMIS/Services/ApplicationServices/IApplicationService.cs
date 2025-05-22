using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services.ApplicationServices
{
    public interface IApplicationService
    {
        Task<Application> CreateApplicationAsync(Application application);
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<List<Application>> GetAllApplicationsAsync();
        Task<Application?> UpdateApplicationAsync(int id, Application updatedApplication);
        Task<bool> ChangeStatusAsync(int id, int statusId);
        Task<bool> DeleteApplicationAsync(int id);
        Task<bool> PermanentlyDeleteApplicationAsync(int id);
    }
}
