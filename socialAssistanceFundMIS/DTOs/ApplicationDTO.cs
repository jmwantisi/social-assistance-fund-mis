public class ApplicationDTO
{
    public int Id { get; set; }

    public DateTime ApplicationDate { get; set; }

    public int ApplicantId { get; set; }
    public string? ApplicantName { get; set; }  // Assuming the applicant has a Name or similar property

    public int ProgramId { get; set; }
    public string? ProgramName { get; set; }  // Assuming the AssistanceProgram has a Name or similar property

    public int StatusId { get; set; }
    public string? StatusName { get; set; }  // Assuming Status has a Name or similar property

    public int OfficialRecordId { get; set; }
    public string? OfficialRecordDetails { get; set; }  // Assuming OfficialRecord has a Details or similar property

    public DateTime DeclarationDate { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }
}
