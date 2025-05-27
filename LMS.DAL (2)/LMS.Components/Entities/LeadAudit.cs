using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
    public class LeadAudit
    {
        [Key]
        public int AuditId { get; set; }
        public int? LeadId { get; set; }
        public string EventType { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventDetails { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedField { get; set; }
        public int? ChangedBy { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
