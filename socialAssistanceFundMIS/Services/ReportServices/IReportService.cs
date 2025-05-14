namespace socialAssistanceFundMIS.Services.ReportServices
{
    public interface IReportService
    {
        Task<(int Pending, int Approved)> GetApplicationStatusCountsAsync();
        Task<(int Male, int Female)> GetApprovedApplicantsByGenderAsync();
        Task<Dictionary<string, int>> GetApplicationCountsPerProgramAsync();

    }
}
