using System.ComponentModel.DataAnnotations;

namespace LMS.Components.ModelClasses.Login
{
	public class LoginRequest
	{
		[Required]
		public string? emailId { get; set; }
		[Required]
		public string? password { get; set; }
	}
}
