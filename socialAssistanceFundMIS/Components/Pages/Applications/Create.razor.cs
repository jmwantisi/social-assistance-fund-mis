using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Services.Applicants;
using socialAssistanceFundMIS.Services.ApplicationServices;
using socialAssistanceFundMIS.Services.LookupServices;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Pages.Applications
{
    public class Create : ComponentBase
    {
        [Parameter]
        public int? Id { get; set; }

        protected ApplicationViewModel ApplicationViewModel { get; set; } = new();
        protected bool IsReadOnly { get; set; } = false;
        protected bool IsEdit => Id.HasValue;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected IApplicationService ApplicationService { get; set; } = default!;

        [Inject] protected ILookupService LookupService { get; set; } = default!;
        [Inject] protected IApplicantService ApplicantService { get; set; } = default!;

        [Inject] protected ApplicationDbContext _context { get; set; } = default!;

        [Parameter]
        public string? Mode { get; set; }  // view, edit, etc.


        protected override async Task OnInitializedAsync()
        {
            IsReadOnly = DetermineReadOnlyMode();

            if (IsEdit)
            {
                // Load the application data based on Id
                var application = await ApplicationService.GetApplicationByIdAsync(Id.Value);
                if (application == null) return; // To do: work on proper response
                ApplicationViewModel = new ApplicationViewModel
                {
                    Id = application.Id,
                    SelectedProgramId = application.ProgramId,
                    ApplicantId = application.ApplicantId,
                    DeclarationDate = application.DeclarationDate,
                    ApplicationDate = application.ApplicationDate,
                    Programs = await LookupService.GetProgramsAsync(),
                    Applicants = await _context.Applicants.Select(a => new Applicant()
                    {
                        Id = a.Id,
                        FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                    }).ToListAsync(),
                };
            }
            else
            {
                await LoadDropdowns();
                ApplicationViewModel = new ApplicationViewModel
                {
                    ApplicationDate = DateOnly.FromDateTime(DateTime.Today), // Fix for CS0029
                    DeclarationDate = DateOnly.FromDateTime(DateTime.Today) // Fix for CS0029
                };
            }
        }

        protected async Task LoadDropdowns()
        {
            ApplicationViewModel = new ApplicationViewModel
            {
                Programs = await LookupService.GetProgramsAsync(),
                Applicants = await _context.Applicants.Select(a => new Applicant()
                {
                    Id = a.Id,
                    FirstName = a.FirstName + " " + (string.IsNullOrEmpty(a.MiddleName) ? "" : a.MiddleName + " ") + a.LastName
                })
                .ToListAsync()
            };
        }

        protected async Task HandleSubmit()
        {
            if (IsEdit)
            {
                var application = new Application
                {
                    ProgramId = ApplicationViewModel.SelectedProgramId,
                    ApplicantId = ApplicationViewModel.ApplicantId,
                    DeclarationDate = ApplicationViewModel.DeclarationDate,
                    ApplicationDate = ApplicationViewModel.ApplicationDate
                };

                application.ProgramId = ApplicationViewModel.SelectedProgramId;
                application.ApplicantId = ApplicationViewModel.ApplicantId;

                await ApplicationService.UpdateApplicationAsync(ApplicationViewModel.Id, application);
            }
            else
            {
                var application = new Application
                {
                    ProgramId = ApplicationViewModel.SelectedProgramId,
                    ApplicantId = ApplicationViewModel.ApplicantId,
                    DeclarationDate = ApplicationViewModel.DeclarationDate,
                    ApplicationDate = ApplicationViewModel.ApplicationDate
                };

                if (application == null) return;

                await ApplicationService.CreateApplicationAsync(application);
            }

            NavigateToList();
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/applications");
        }

        protected bool DetermineReadOnlyMode()
        {
            return string.Equals(Mode, "view", StringComparison.OrdinalIgnoreCase);
        }
    }
}
