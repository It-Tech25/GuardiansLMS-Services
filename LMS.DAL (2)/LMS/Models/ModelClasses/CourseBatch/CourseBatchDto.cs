using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.CourseBatch
{
    public class CourseBatchDto
    {
        public int BatchId { get; set; } // Required for Edit/Delete
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Duration { get; set; } // In days or weeks
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int CreatedBy { get; set; }
    }
    public class CourseBatchDtos
    {
        public int BatchId { get; set; } // Required for Edit/Delete
        public string Course { get; set; }
        public string  Instructor { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Duration { get; set; } // In days or weeks
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int CreatedBy { get; set; }
    }


}
