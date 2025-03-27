using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services
{
    public interface IRegistrationService
    {
        Task<bool> SubmitApplication(Application application);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public RegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SubmitApplication(Application application)
        {
            _context.Applications.Add(application);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
