namespace socialAssistanceFundMIS.Services.PhoneNumberTypeServices
{
    public interface IPhoneNumberTypeService
    {
        Task<PhoneNumberTypeDTO> CreatePhoneNumberTypeAsync(PhoneNumberTypeDTO dto);
        Task<PhoneNumberTypeDTO?> GetPhoneNumberTypeByIdAsync(int id);
        Task<List<PhoneNumberTypeDTO>> GetAllPhoneNumberTypesAsync();
        Task<PhoneNumberTypeDTO> UpdatePhoneNumberTypeAsync(int id, PhoneNumberTypeDTO dto);
        Task<bool> DeletePhoneNumberTypeAsync(int id);
        Task<bool> PermanentlyDeletePhoneNumberTypeAsync(int id);
    }
}
