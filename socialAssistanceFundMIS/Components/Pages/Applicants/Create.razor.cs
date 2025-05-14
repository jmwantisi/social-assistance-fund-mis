using Microsoft.AspNetCore.Components;
using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using System.Reflection.Metadata;
using socialAssistanceFundMIS.Services.LookupServices;
using socialAssistanceFundMIS.Services.GeographicLocationServices;
using socialAssistanceFundMIS.ViewModels;
using socialAssistanceFundMIS.Services.Applicants;
using socialAssistanceFundMIS.Helpers;

namespace socialAssistanceFundMIS.Pages.Applicants;

public partial class CreateBase : ComponentBase
{
    [Parameter]
    public int? Id { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "readonly")]
    public bool IsReadOnly { get; set; }

    protected ApplicantViewModel Applicant { get; set; } = new();
    public List<(string phoneNumber, int phoneNumberTypeId)> PhoneNumbers { get; set; } = new();
    public bool IsEdit => Id.HasValue;

    [Inject] protected ApplicationDbContext DbContext { get; set; } = default!;
    [Inject] protected ILookupService LookupService { get; set; } = default!;
    [Inject] protected IGeographicLocationService GeographicLocationService { get; set; } = default!;
    [Inject] protected IApplicantService ApplicantService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        // Applicant.PhoneNumbers ??= new List<ApplicantPhoneNumber>();

        if (Id.HasValue)
        {
            var applicantVm = await ApplicantService.GetApplicantByIdAsync(Id.Value);
            if (applicantVm != null)
            {
                Applicant = applicantVm;
                // Map phone numbers to editable list
                Applicant.PhoneNumbers = applicantVm.PhoneNumbers.Select(p => new ApplicantPhoneNumber
                {
                    PhoneNumber = p.PhoneNumber,
                    PhoneNumberTypeId = p.PhoneNumberTypeId
                }).ToList();

                await LoadDropdownData();
            }
        }
        else
        {
            await LoadDropdownData();
            Applicant.PhoneNumbers = new List<ApplicantPhoneNumber> { new() };
        }
    }

    private async Task LoadDropdownData()
    {
        Applicant.Sexes = await LookupService.GetSexesAsync();
        Applicant.MaritalStatuses = await LookupService.GetMaritalStatusesAsync();
        Applicant.Villages = await GeographicLocationService.GetVillagesWithHierarchyAsync();
        Applicant.PhoneNumberTypes = await LookupService.GetPhoneNumberTypesAsync() ?? new List<PhoneNumberType>();
    }

    protected void AddPhoneNumber()
    {
        Applicant.PhoneNumbers.Add(new ApplicantPhoneNumber());
    }

    protected void RemovePhoneNumber(int index)
    {
        if (Applicant.PhoneNumbers.Count > 1)
        {
            Applicant.PhoneNumbers.RemoveAt(index);
        }
    }

    protected async Task HandleSubmit()
    {
        var applicantEntity = ApplicantMapper.ToApplicantEntity(Applicant);

        if (!IsEdit)
        {
            var result = await ApplicantService.CreateApplicantAsync(applicantEntity,
                Applicant.PhoneNumbers.Select(p => (p.PhoneNumber, p.PhoneNumberTypeId)).ToList());

            if (result != null)
            {
                NavigationManager.NavigateTo("/applicants");
            }
        }
        else
        {
            var result = await ApplicantService.UpdateApplicantAsync(
                Id.Value,
                applicantEntity,
                Applicant.PhoneNumbers.Select(p => (p.PhoneNumber, p.PhoneNumberTypeId)).ToList()
            );

            if (result != null)
            {
                NavigationManager.NavigateTo("/applicants");
            }
        }
    }


    protected void Cancel()
    {
        Console.WriteLine("Create button clicked");
        NavigationManager.NavigateTo("/applicants");
    }
}
