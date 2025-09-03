using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using socialAssistanceFundMIS.Common;

namespace socialAssistanceFundMIS.Models
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(Constants.Validation.MaxNameLength, ErrorMessage = "First Name cannot exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "First Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(Constants.Validation.MaxNameLength, ErrorMessage = "Middle Name cannot exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z\s\-']*$", ErrorMessage = "Middle Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(Constants.Validation.MaxNameLength, ErrorMessage = "Last Name cannot exceed {1} characters")]
        [RegularExpression(@"^[a-zA-Z\s\-']+$", ErrorMessage = "Last Name can only contain letters, spaces, hyphens, and apostrophes")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(Constants.Validation.MaxEmailLength, ErrorMessage = "Email cannot exceed {1} characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public int SexId { get; set; }
        
        [ForeignKey(nameof(SexId))]
        public virtual Sex? Sex { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        public int MaritalStatusId { get; set; }
        
        [ForeignKey(nameof(MaritalStatusId))]
        public virtual MaritalStatus? MaritalStatus { get; set; }

        [ForeignKey(nameof(VillageId))]
        public int? VillageId { get; set; }
        public virtual GeographicLocation? Village { get; set; }

        [Required(ErrorMessage = "Identity Card Number is required")]
        [StringLength(Constants.Validation.MaxIdentityCardLength, ErrorMessage = "Identity Card Number cannot exceed {1} characters")]
        [RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Identity Card Number can only contain uppercase letters and numbers")]
        [Display(Name = "Identity Card Number")]
        public string IdentityCardNumber { get; set; } = string.Empty;

        [InverseProperty(nameof(ApplicantPhoneNumber.Applicant))]
        public virtual ICollection<ApplicantPhoneNumber> PhoneNumbers { get; set; } = new List<ApplicantPhoneNumber>();

        [StringLength(Constants.Validation.MaxAddressLength, ErrorMessage = "Postal Address cannot exceed {1} characters")]
        [Display(Name = "Postal Address")]
        public string? PostalAddress { get; set; }

        [Required(ErrorMessage = "Physical Address is required")]
        [StringLength(Constants.Validation.MaxAddressLength, ErrorMessage = "Physical Address cannot exceed {1} characters")]
        [Display(Name = "Physical Address")]
        public string PhysicalAddress { get; set; } = string.Empty;

        [InverseProperty(nameof(Application.Applicant))]
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

        // Audit fields
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        // Computed properties
        [NotMapped]
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - DateOfBirth.Year;
                if (today < new DateOnly(today.Year, DateOfBirth.Month, DateOfBirth.Day))
                    age--;
                return age;
            }
        }

        // Validation method
        public bool IsValidAge()
        {
            return Age >= Constants.Validation.MinAge && Age <= Constants.Validation.MaxAge;
        }
    }
}
