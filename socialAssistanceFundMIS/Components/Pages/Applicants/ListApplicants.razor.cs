using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using socialAssistanceFundMIS.ViewModels;
using socialAssistanceFundMIS.Services.Applicants;

namespace socialAssistanceFundMIS.Pages.Applicants // Make sure this matches the @inherits
{
    public partial class ListApplicantsBase : ComponentBase
    {
        protected List<ApplicantViewModel>? applicants; // Make it protected so Razor can access it

        [Inject] protected IApplicantService ApplicantService { get; set; } = default!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            applicants = await ApplicantService.GetAllApplicantsAsync();
        }

        protected void NavigateToCreateApplicant()
        {
            NavigationManager.NavigateTo("/applicants/create");
        }

        protected void EditApplicant(int id)
        {
            NavigationManager.NavigateTo($"/applicants/edit/{id}");
        }

        protected async Task ConfirmDelete(int id)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this applicant?");
            if (confirmed)
            {
                await ApplicantService.DeleteApplicantAsync(id);
                applicants = await ApplicantService.GetAllApplicantsAsync();
            }
        }
    }
}
