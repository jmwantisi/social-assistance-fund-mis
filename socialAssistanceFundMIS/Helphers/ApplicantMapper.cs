using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.ViewModels;

namespace socialAssistanceFundMIS.Helpers
{
    public static class ApplicantMapper
    {
        public static Applicant ToApplicantEntity(ApplicantViewModel vm)
        {
            return new Applicant
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                MiddleName = vm.MiddleName,
                LastName = vm.LastName,
                Email = vm.Email,
                SexId = vm.SexId,
                Dob = vm.Dob,
                MaritialStatusId = vm.MaritalStatusId,
                VillageId = vm.VillageId,
                IdentityCardNumber = vm.IdentityCardNumber,
                PhoneNumbers = vm.PhoneNumbers, // assuming this is properly populated
                PostalAddress = vm.PostalAddress,
                PhysicalAddress = vm.PhysicalAddress,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static ApplicantViewModel ToApplicantViewModel(Applicant entity)
        {
            return new ApplicantViewModel
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                Email = entity.Email,
                SexId = entity.SexId,
                Dob = entity.Dob,
                MaritalStatusId = entity.MaritialStatusId,
                VillageId = entity.VillageId,
                IdentityCardNumber = entity.IdentityCardNumber,
                PhoneNumbers = entity.PhoneNumbers,
                PostalAddress = entity.PostalAddress,
                PhysicalAddress = entity.PhysicalAddress
            };
        }
    }
}
