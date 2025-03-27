using socialAssistanceFundMIS.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace socialAssistanceFundMIS.Models
{
    public class ApplicantProgram
    {
        [Key]
        public int Id { get; set; }

        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public int ProgramId { get; set; }
        public AssistanceProgram? Program { get; set; }
        
        public DateTime EnrolledDate { get; set; }

        public int Removed { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }

}
