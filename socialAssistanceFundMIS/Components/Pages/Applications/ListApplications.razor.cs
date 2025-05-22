using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Services.ApplicationServices;

namespace socialAssistanceFundMIS.Pages.Applications
{
    public partial class ListApplications : ComponentBase
    {
        protected List<Application>? applications;

        protected DateTime? minDate;
        protected DateTime? maxDate;

        [Inject] protected IApplicationService ApplicationService { get; set; } = default!;
        [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
        [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            applications = await ApplicationService.GetAllApplicationsAsync();
        }

        protected IEnumerable<Application> FilteredApplications =>
            applications?.Where(app =>
                (!minDate.HasValue || app.ApplicationDate >= DateOnly.FromDateTime(minDate.Value)) &&
                (!maxDate.HasValue || app.ApplicationDate <= DateOnly.FromDateTime(maxDate.Value))
            ) ?? Enumerable.Empty<Application>();


        protected void NavigateToCreateApplication()
        {
            NavigationManager.NavigateTo("/applications/create");
        }

        protected void EditApplication(int id)
        {
            NavigationManager.NavigateTo($"/applications/edit/{id}");
        }

        protected void ViewApplication(int id)
        {
            NavigationManager.NavigateTo($"/applications/view/{id}");
        }

        protected async Task ConfirmDelete(int id)
        {
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this application?");
            if (confirmed)
            {
                await ApplicationService.DeleteApplicationAsync(id);
                applications = await ApplicationService.GetAllApplicationsAsync();
            }
        }

        protected async Task ApproveApplication(int id, int currentStatusId)
        {
            var newStatusId = currentStatusId == 1 ? 2 : 1;
            var confirmMessage = currentStatusId == 1
                ? "Are you sure you want to approve this application?"
                : "Are you sure you want to disapprove this application?";

            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", confirmMessage);
            if (confirmed)
            {
                await ApplicationService.ChangeStatusAsync(id, newStatusId);
                applications = await ApplicationService.GetAllApplicationsAsync();
            }
        }
    }
}
