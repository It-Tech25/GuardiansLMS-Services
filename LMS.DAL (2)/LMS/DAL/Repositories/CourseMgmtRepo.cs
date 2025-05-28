using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class CourseMgmtRepo : ICourseMgmtRepo
    {
        private readonly MyDbContext context;
        public CourseMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        #region Course Type

        public List<CourseTypesDTO> GetCourseTypeList(string search = "")
        {
            List<CourseTypesDTO> response = new List<CourseTypesDTO>();
            try
            {
                response = (from m in context.courseTypes
                            where m.IsDeleted == false && m.CourseName.Contains(search)
                            select new CourseTypesDTO
                            {
                                CourseId = m.CourseId,
                                CourseName = m.CourseName,
                                Description = m.Description,
                                CourseImgURL = m.CourseImgURL,
                                Duration = m.Duration,
                                CoursePrice = m.CoursePrice,
                                ContactNumber = m.ContactNumber,
                                CourseCode = m.CourseCode,
                                ProfessorName = m.ProfessorName,
                                StartFrom = m.StartFrom,
                                MaxCount = m.MaxCount,
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }
        public List<CourseDropdownDTO> GetCourseListDropDown()
        {
            List<CourseDropdownDTO> response = new List<CourseDropdownDTO>();
            try
            {
                response = (from m in context.courseTypes
                            where m.IsDeleted == false
                            select new CourseDropdownDTO
                            {
                                CourseId = m.CourseId,
                                CourseName = m.CourseName,
                               
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public List<CourseTypeDTO> GetCourseType()
        {
            List<CourseTypeDTO> response = new List<CourseTypeDTO>();
            try
            {
                response = (from m in context.courseTypes
                            where m.IsDeleted == false && m.CourseId == null && m.IsActive == true
                            select new CourseTypeDTO
                            {
                                CourseId = m.CourseId,
                                CourseName = m.CourseName
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public CourseTypeByIdDTO GetCourseTypeById(int id)
        {
            CourseTypeByIdDTO response = new CourseTypeByIdDTO();
            try
            {
                response = (from m in context.courseTypes
                            where m.IsDeleted == false && m.CourseId == id 
                            select new CourseTypeByIdDTO
                            {
                                CourseId = m.CourseId,
                                CourseName = m.CourseName,
                                Description = m.Description,
                                CourseImgURL = m.CourseImgURL,
                                Duration = m.Duration,
                                CoursePrice = m.CoursePrice,
                                ContactNumber = m.ContactNumber,
                                CourseCode = m.CourseCode,
                                ProfessorName = m.ProfessorName,
                                StartFrom = m.StartFrom,
                                MaxCount = m.MaxCount,
                            }).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public CourseTypesEntity GetCourseTypesById(int id)
        {
            CourseTypesEntity response = new CourseTypesEntity();
            try
            {
                response = context.courseTypes.Where(m => m.CourseId == id && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public GenericResponse AddCourseType(CourseTypesEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.courseTypes.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.CourseId;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateCourseType(CourseTypesEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.courseTypes.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.CourseId;
            }
            catch (Exception ex)
            {
                response.statusCode = 404;
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion
    }
}
