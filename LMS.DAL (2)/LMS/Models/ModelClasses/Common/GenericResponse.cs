namespace LMS.Components.ModelClasses.Common
{
	public class GenericResponse
	{
		public string? Message { get; set; }
		public int statusCode { get; set; }
		public int? CurrentId { get; set; }
	}
}
