using System.ComponentModel.DataAnnotations;

namespace socialAssistanceFundMIS.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        
        [StringLength(100)]
        public string? UpdatedBy { get; set; }

        public virtual void UpdateAuditFields(string? updatedBy = null)
        {
            UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(updatedBy))
                UpdatedBy = updatedBy;
        }
    }
}

