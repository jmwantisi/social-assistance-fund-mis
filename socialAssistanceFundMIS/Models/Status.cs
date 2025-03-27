namespace socialAssistanceFundMIS.Models
{
    public class Status
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Removed { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }
}
