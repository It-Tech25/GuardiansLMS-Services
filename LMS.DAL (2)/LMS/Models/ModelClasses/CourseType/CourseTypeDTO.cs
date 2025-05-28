using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.CourseType
{
    public class CourseTypeDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public DateTime? StartFrom { get; set; }
        public decimal? Duration { get; set; }
        public decimal? CoursePrice { get; set; }
        public string ProfessorName { get; set; }
        public int? MaxCount { get; set; }
        public string ContactNumber { get; set; }
        public string CourseImgURL { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? DeactivatedBy { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public bool? IsDeleted { get; set; }

    }

     
    public class CourseTypesDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public DateTime? StartFrom { get; set; }
        public decimal? Duration { get; set; }
        public decimal? CoursePrice { get; set; }
        public string ProfessorName { get; set; }
        public int? MaxCount { get; set; }
        public string ContactNumber { get; set; }
        public string CourseImgURL { get; set; }
      

    }

    public class CourseTypeByIdDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Description { get; set; }
        public DateTime? StartFrom { get; set; }
        public decimal? Duration { get; set; }
        public decimal? CoursePrice { get; set; }
        public string ProfessorName { get; set; }
        public int? MaxCount { get; set; }
        public string ContactNumber { get; set; }
        public string CourseImgURL { get; set; }



    }
    public class CourseDropdownDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
  


    }

}
