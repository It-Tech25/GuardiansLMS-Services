using LMS.BAL.Interfaces;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.Components.Utilities;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CourseTypeController : Controller
    {

        private readonly ICourseMgmtService mService;

        public CourseTypeController(ICourseMgmtService _mservice)
        {
            mService = _mservice;
        }

        #region Course

       
        [HttpGet]
        public IActionResult GetCourseList(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            var allCourses = mService.GetCourseTypeList(); 
            var filtered = allCourses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                filtered = filtered.Where(x => x.CourseName.ToLower().Contains(term));
            }

            int skip = (pagenumber - 1) * pagesize;
            filtered = filtered.Skip(skip).Take(pagesize);

            var finalList = filtered.ToList();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(finalList);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = filtered.Count() + skip; 

            return Ok(finalResponse);
        }



        [HttpGet("GetCourseById")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            CourseTypeByIdDTO res = new CourseTypeByIdDTO();
            res = mService.GetCourseTypeById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPost("AddEditCourse")]
        public async Task<IActionResult> AddEditCourse(CourseTypesDTO req)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.AddEditModule(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("ActivateCourse")]
        public async Task<IActionResult> ActivateCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.ActivateCourseType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeActivateCourse")]
        public async Task<IActionResult> DeActivateCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeActivateCourseType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeleteCourseType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        #endregion



    }
}
