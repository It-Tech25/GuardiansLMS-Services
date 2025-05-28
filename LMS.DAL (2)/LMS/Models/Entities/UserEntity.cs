using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
	public class UserEntity
	{
		[Key]
		public int UserId { get; set; }

		public string? UserName { get; set; }

		public string? PWord { get; set; }

		public string? EmailId { get; set; }

		public string? MobileNumber { get; set; }

		public string? ProfileUrl { get; set; }

		public string? UserCode { get; set; }

		public int? UserType { get; set; }

		public DateTime? CreatedOn { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? ActivatedOn { get; set; }

		public int? ActivatedBy { get; set; }

		public bool? IsActive { get; set; }

		public bool? IsDeleted { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime? RefTokenExpDate { get; set; }
	}
}
