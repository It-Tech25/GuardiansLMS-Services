﻿namespace LMS.Components.Utilities
{
	public class ApiResponse<T>
	{
		public T Response { get; set; }
		public bool Succeded { get; set; } = false;
		public IEnumerable<string> Errors { get; set; }
		public int? totalRecords { get; set; } = 0;
		public int? fromRecord { get; set; } = 0;
		public int? torecord { get; set; } = 0;
		public int unReadCount { get; set; } = 0;

	}
}
