using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services.LookupServices
{
    public interface ILookupService
    {
        Task<IEnumerable<AssistanceProgram>> GetProgramsAsync();
        Task<AssistanceProgram?> GetProgramByIdAsync(int id);
        Task<IEnumerable<Sex>> GetSexesAsync();
        Task<Sex?> GetSexByIdAsync(int id);
        Task<IEnumerable<MaritalStatus>> GetMaritalStatusesAsync();
        Task<MaritalStatus?> GetMaritalStatusByIdAsync(int id);
        Task<IEnumerable<Status>> GetStatusesAsync();
        Task<Status?> GetStatusByIdAsync(int id);
        Task<IEnumerable<PhoneNumberType>> GetPhoneNumberTypesAsync();
        Task<PhoneNumberType?> GetPhoneNumberTypeByIdAsync(int id);
    }
}
