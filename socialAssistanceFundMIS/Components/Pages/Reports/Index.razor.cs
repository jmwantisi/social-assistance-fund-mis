using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using socialAssistanceFundMIS.Services.ReportServices;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Pages.Reports
{
    public partial class Index : ComponentBase
    {
        [Inject] protected IReportService ReportService { get; set; } = default!;

        protected ReportViewModel ReportViewModel { get; set; } = new();

        protected PieChart statusPieChart = default!;
        protected PieChart genderPieChart = default!;
        protected BarChart programBarChart = default!;

        protected PieChartOptions statusPieOptions = default!;
        protected PieChartOptions genderPieOptions = default!;
        protected BarChartOptions programBarOptions = default!;

        protected ChartData statusChartData = default!;
        protected ChartData genderChartData = default!;
        protected ChartData programChartData = default!;
        private bool _chartsInitialized;


        protected readonly string[] backgroundColors = { "#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0", "#9966FF", "#FF9F40" };

        protected override async Task OnInitializedAsync()
        {
            // Fetch data from the service
            var (pending, approved) = await ReportService.GetApplicationStatusCountsAsync();
            var (male, female) = await ReportService.GetApprovedApplicantsByGenderAsync();
            var perProgram = await ReportService.GetApplicationCountsPerProgramAsync();

            // Populate ReportViewModel
            ReportViewModel = new ReportViewModel
            {
                StatusData = new[] { pending, approved, 0, 0 }, // Rejected and Withdrawn set to 0 temporarily
                GenderData = new[] { male, female, 0 }, // Other gender set to 0
                ProgramLabels = perProgram.Keys.ToList(),
                ProgramData = perProgram.Values.ToList()
            };

            InitializeChartOptions();
            BuildCharts();

            await base.OnInitializedAsync();
        }

        private void InitializeChartOptions()
        {
            statusPieOptions = new PieChartOptions();
            statusPieOptions.Responsive = true;
            statusPieOptions.Plugins.Title!.Text = "Applications by Status";
            statusPieOptions.Plugins.Title.Display = true;

            genderPieOptions = new PieChartOptions();
            genderPieOptions.Responsive = true;
            genderPieOptions.Plugins.Title!.Text = "Approved Applicants by Gender";
            genderPieOptions.Plugins.Title.Display = true;

            programBarOptions = new BarChartOptions();
            programBarOptions.Responsive = true;
            programBarOptions.Plugins.Title!.Text = "Applications per Program";
            programBarOptions.Plugins.Title.Display = true;
            programBarOptions.Scales!.Y!.BeginAtZero = true;
        }

        private void BuildCharts()
        {
            // Applications by Status Pie Chart
            statusChartData = new ChartData
            {
                Labels = new List<string> { "Pending", "Approved", "Rejected" },
                Datasets = new List<IChartDataset>
                {
                    new PieChartDataset
                    {
                        Label = "Applications by Status",
                        Data = ReportViewModel.StatusData.Select(x => (double?)x).ToList(),
                        BackgroundColor = backgroundColors.Take(ReportViewModel.StatusData.Length).ToList()
                    }
                }
            };

            // Approved Applicants by Gender Pie Chart
            genderChartData = new ChartData
            {
                Labels = new List<string> { "Male", "Female" },
                Datasets = new List<IChartDataset>
                {
                    new PieChartDataset
                    {
                        Label = "Approved Applicants by Gender",
                        Data = ReportViewModel.GenderData.Select(x => (double?)x).ToList(),
                        BackgroundColor = backgroundColors.Take(ReportViewModel.GenderData.Length).ToList()
                    }
                }
            };

            // Applications per Program Bar Chart
            programChartData = new ChartData
            {
                Labels = ReportViewModel.ProgramLabels,
                Datasets = new List<IChartDataset>
                {
                    new BarChartDataset
                    {
                        Label = "Applications per Program",
                        Data = ReportViewModel.ProgramData.Select(x => (double?)x).ToList(),
                        BackgroundColor = backgroundColors.Take(ReportViewModel.ProgramData.Count).ToList()
                    }
                }
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_chartsInitialized)
            {
                if (statusPieChart != null && genderPieChart != null && programBarChart != null)
                {
                    await Task.Delay(100); // Not recommended. Had an issue with delays in the rendering engine. Temp fix
                    await statusPieChart.InitializeAsync(statusChartData, statusPieOptions);
                    await genderPieChart.InitializeAsync(genderChartData, genderPieOptions);
                    await programBarChart.InitializeAsync(programChartData, programBarOptions);

                    _chartsInitialized = true;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

    }
}
