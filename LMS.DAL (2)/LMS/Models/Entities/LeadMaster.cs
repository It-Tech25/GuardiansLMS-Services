using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
    public class LeadMaster
    {
        [Key]
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }
        public int? AssignedUserId { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? AssignedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? LastFollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public DateTime? NegotiationDate { get; set; }
        public DateTime? DemoDate { get; set; }
        public string? ClosedReason { get; set; }
    }
}
