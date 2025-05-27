using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BAL.Interfaces
{
    public interface ICourseMgmtService
    {


        List<CourseTypesDTO> GetCourseTypeList(string search = "");

        List<CourseTypeDTO> GetCourseType();
        CourseTypeByIdDTO GetCourseTypeById(int id);
        GenericResponse AddEditModule(CourseTypesDTO req, int currentUserId);

        GenericResponse ActivateCourseType(int id, int currentUserId);
        GenericResponse DeActivateCourseType(int id, int currentUserId);
        GenericResponse DeleteCourseType(int id, int currentUserId);
    }
}
