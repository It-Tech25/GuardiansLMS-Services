using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Interfaces
{
    public interface ICourseMgmtRepo
    {
        #region Course Type

        List<CourseTypesDTO> GetCourseTypeList(string search = "");
        List<CourseTypeDTO> GetCourseType();
        CourseTypeByIdDTO GetCourseTypeById(int id);
        CourseTypesEntity GetCourseTypesById(int id);
        GenericResponse AddCourseType(CourseTypesEntity req);
        GenericResponse UpdateCourseType(CourseTypesEntity req);


        #endregion
    }
}
