using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
	public class UserTypeMasterEntity
	{
		[Key]
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? DeactivatedBy { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public bool? IsUsable { get; set; }
    }
}
