public class ApplicantPhoneNumberDTO
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; }
    public int PhoneNumberTypeId { get; set; }
    public string PhoneNumberType { get; set; }  // Add human-readable phone number type
}