using FluentValidation;
using socialAssistanceFundMIS.Models;
using socialAssistanceFundMIS.Common;

namespace socialAssistanceFundMIS.Infrastructure.Validation
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required")
                .MaximumLength(Constants.Validation.MaxNameLength)
                .WithMessage($"First Name cannot exceed {Constants.Validation.MaxNameLength} characters")
                .Matches(@"^[a-zA-Z\s\-']+$")
                .WithMessage("First Name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.MiddleName)
                .MaximumLength(Constants.Validation.MaxNameLength)
                .WithMessage($"Middle Name cannot exceed {Constants.Validation.MaxNameLength} characters")
                .Matches(@"^[a-zA-Z\s\-']*$")
                .WithMessage("Middle Name can only contain letters, spaces, hyphens, and apostrophes")
                .When(x => !string.IsNullOrEmpty(x.MiddleName));

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .MaximumLength(Constants.Validation.MaxNameLength)
                .WithMessage($"Last Name cannot exceed {Constants.Validation.MaxNameLength} characters")
                .Matches(@"^[a-zA-Z\s\-']+$")
                .WithMessage("Last Name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(Constants.Validation.MaxEmailLength)
                .WithMessage($"Email cannot exceed {Constants.Validation.MaxEmailLength} characters")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.SexId)
                .GreaterThan(0).WithMessage("Sex is required");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .Must(BeValidDateOfBirth).WithMessage("Date of Birth must be a valid date")
                .Must(BeWithinReasonableRange).WithMessage($"Age must be between {Constants.Validation.MinAge} and {Constants.Validation.MaxAge} years");

            RuleFor(x => x.MaritalStatusId)
                .GreaterThan(0).WithMessage("Marital Status is required");

            RuleFor(x => x.IdentityCardNumber)
                .NotEmpty().WithMessage("Identity Card Number is required")
                .MaximumLength(Constants.Validation.MaxIdentityCardLength)
                .WithMessage($"Identity Card Number cannot exceed {Constants.Validation.MaxIdentityCardLength} characters")
                .Matches(@"^[A-Z0-9]+$")
                .WithMessage("Identity Card Number can only contain uppercase letters and numbers");

            RuleFor(x => x.PostalAddress)
                .MaximumLength(Constants.Validation.MaxAddressLength)
                .WithMessage($"Postal Address cannot exceed {Constants.Validation.MaxAddressLength} characters")
                .When(x => !string.IsNullOrEmpty(x.PostalAddress));

            RuleFor(x => x.PhysicalAddress)
                .NotEmpty().WithMessage("Physical Address is required")
                .MaximumLength(Constants.Validation.MaxAddressLength)
                .WithMessage($"Physical Address cannot exceed {Constants.Validation.MaxAddressLength} characters");

            RuleFor(x => x.PhoneNumbers)
                .Must(HaveValidPhoneNumbers).WithMessage("At least one valid phone number is required");

            RuleForEach(x => x.PhoneNumbers)
                .SetValidator(new ApplicantPhoneNumberValidator());
        }

        private bool BeValidDateOfBirth(DateOnly dateOfBirth)
        {
            return dateOfBirth != default && dateOfBirth <= DateOnly.FromDateTime(DateTime.Today);
        }

        private bool BeWithinReasonableRange(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dateOfBirth.Year;
            if (today < new DateOnly(today.Year, dateOfBirth.Month, dateOfBirth.Day))
                age--;

            return age >= Constants.Validation.MinAge && age <= Constants.Validation.MaxAge;
        }

        private bool HaveValidPhoneNumbers(ICollection<ApplicantPhoneNumber> phoneNumbers)
        {
            return phoneNumbers != null && phoneNumbers.Any() && 
                   phoneNumbers.All(pn => !string.IsNullOrEmpty(pn.PhoneNumber) && pn.PhoneNumberTypeId > 0);
        }
    }

    public class ApplicantPhoneNumberValidator : AbstractValidator<ApplicantPhoneNumber>
    {
        public ApplicantPhoneNumberValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required")
                .MaximumLength(Constants.Validation.MaxPhoneNumberLength)
                .WithMessage($"Phone Number cannot exceed {Constants.Validation.MaxPhoneNumberLength} characters")
                .Matches(@"^[\d\-\+\(\)\s]+$")
                .WithMessage("Phone Number can only contain digits, hyphens, plus signs, parentheses, and spaces");

            RuleFor(x => x.PhoneNumberTypeId)
                .GreaterThan(0).WithMessage("Phone Number Type is required");
        }
    }
}

