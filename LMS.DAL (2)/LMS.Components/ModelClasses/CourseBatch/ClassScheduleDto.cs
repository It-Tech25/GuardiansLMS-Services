using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.CourseBatch
{
    public class ClassScheduleDto
    {
        public int ScheduleId { get; set; } 
        public int BatchId { get; set; }
        public DateTime ClassDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? Topic { get; set; }
        public string? Remarks { get; set; }
    }

}
