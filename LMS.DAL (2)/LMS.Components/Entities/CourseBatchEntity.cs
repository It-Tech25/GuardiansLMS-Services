using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.Entities
{
    public class CourseBatchEntity
    {
        [Key]
        public int BatchId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int InstructorId { get; set; }

        public DateTime? StartDate { get; set; }

        [MaxLength(100)]
        public int? Duration { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }

}
