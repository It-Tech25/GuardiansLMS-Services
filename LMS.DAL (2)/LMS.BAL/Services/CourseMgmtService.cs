using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.DAL.Interfaces;

namespace LMS.BAL.Services
{
    public class CourseMgmtService : ICourseMgmtService
    {
        private readonly ICourseMgmtRepo mRepo;

        public CourseMgmtService(ICourseMgmtRepo mRepo)
        {
            this.mRepo = mRepo;
        }

         
        #region Course Type

        public List<CourseTypesDTO> GetCourseTypeList(string search = "")
        {
            return mRepo.GetCourseTypeList(search);
        }
        public List<CourseDropdownDTO> GetCourseListDropDown()
        {
            return mRepo.GetCourseListDropDown();
        }

        public List<CourseTypeDTO> GetCourseType()
        {
            return mRepo.GetCourseType();
        }

        public CourseTypeByIdDTO GetCourseTypeById(int id)
        {
            return mRepo.GetCourseTypeById(id);
        }

        public GenericResponse AddEditModule(CourseTypesDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            CourseTypesEntity m = new CourseTypesEntity();

            if (req.CourseId == 0)
            {
                m.CourseName = req.CourseName;
                m.ContactNumber = req.ContactNumber; 
                m.Description = req.Description;
                m.CourseImgURL = req.CourseImgURL;
                m.CourseCode = req.CourseCode; 
                m.CoursePrice = req.CoursePrice;
                m.MaxCount = req.MaxCount;
                m.Duration = req.Duration;
                m.ProfessorName = req.ProfessorName; 
                m.StartFrom = req.StartFrom;
                m.CreatedBy = currentUserId;
                m.CreatedOn = DateTime.Now;
                m.IsActive = true;
                m.IsDeleted = false;
                res = mRepo.AddCourseType(m);
            }
            else
            {
                m = mRepo.GetCourseTypesById(req.CourseId);
                if (m.CourseId == 0)
                {
                    res.statusCode = 0;
                    res.Message = "Id does not exists";
                }
                else
                {
                    m.CourseName = req.CourseName;
                    m.ContactNumber = req.CourseName;
                    m.Description = req.Description;
                    m.CourseImgURL = req.CourseImgURL;
                    m.CourseCode = req.CourseCode;
                    m.CoursePrice = req.CoursePrice;
                    m.MaxCount = req.MaxCount;
                    m.Duration = req.Duration;
                    m.StartFrom = req.StartFrom;
                    m.ProfessorName = req.ProfessorName;
                    m.ModifiedBy = currentUserId;
                    m.ModifiedOn = DateTime.Now;
                    res = mRepo.UpdateCourseType(m);
                }
            }
            return res;
        }

         
        public GenericResponse ActivateCourseType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            CourseTypesEntity m = mRepo.GetCourseTypesById(id);
            if (m.CourseId != 0)
            {
                if (m.IsActive == true)
                {
                    res.statusCode = 0;
                    res.Message = "The course is already Active";
                }
                else
                {
                    m.IsActive = true;
                    res = mRepo.UpdateCourseType(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public GenericResponse DeActivateCourseType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            CourseTypesEntity m = mRepo.GetCourseTypesById(id);
            if (m.CourseId != 0)
            {
                if (m.IsActive == false)
                {
                    res.statusCode = 0;
                    res.Message = "The course is already De activated";
                }
                else
                {
                    m.IsActive = false;
                    m.DeactivatedBy = currentUserId;
                    m.DeactivatedOn = DateTime.Now;
                    res = mRepo.UpdateCourseType(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public GenericResponse DeleteCourseType(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            CourseTypesEntity m = mRepo.GetCourseTypesById(id);
            if (m.CourseId != 0)
            {
                m.IsDeleted = true;
                m.ModifiedBy = currentUserId;
                m.ModifiedOn = DateTime.Now;
                res = mRepo.UpdateCourseType(m);
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        #endregion




    }
}
