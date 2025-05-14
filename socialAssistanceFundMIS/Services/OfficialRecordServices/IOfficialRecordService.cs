using socialAssistanceFundMIS.Models;

namespace socialAssistanceFundMIS.Services.OfficialRecordServices
{
    public interface IOfficialRecordService
    {
        Task<OfficialRecord> CreateOfficialRecordAsync();
        Task<OfficialRecordDTO?> GetOfficialRecordByIdAsync(int id);
        Task<List<OfficialRecordDTO>> GetAllOfficialRecordsAsync();
        Task<OfficialRecordDTO> UpdateOfficialRecordAsync(int id, OfficialRecordDTO dto);
        Task<bool> DeleteOfficialRecordAsync(int id);
        Task<bool> PermanentlyDeleteOfficialRecordAsync(int id);
    }
}
