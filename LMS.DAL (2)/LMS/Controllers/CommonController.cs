using LMS.BAL.Interfaces;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.Instructor;
using LMS.Components.ModelClasses.Student;
using LMS.Components.ModelClasses.UserDTOs;
using LMS.Components.Utilities;
using LMS.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICourseMgmtService mService;
        private readonly IUserMgmtService uService;
        private readonly IStudentRepository repo;
        private readonly IInstructorRepo irepo;

        public CommonController(ICourseMgmtService _mservice, IUserMgmtService uService,IStudentRepository _repo,IInstructorRepo _irepo)
        {
            mService = _mservice;
            this.uService = uService;
            repo= _repo;
            irepo = _irepo;
        }
        [HttpGet("GetCourseDropDown")]
        public async Task<IActionResult> GetCourseListDropDown()
        {
            List<CourseDropdownDTO> res = new List<CourseDropdownDTO>();
            res = mService.GetCourseListDropDown();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
        [HttpGet("GetUsertypeDropDown")]
        public async Task<IActionResult> GetUsertypeDropDown()
        {
            List<UsertDTO> res = new List<UsertDTO>();
            res = uService.GetUserTypeDD();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
        [HttpGet("GetStudentDropDown")]
        public async Task<IActionResult> GetStudentDropDown()
        {
            List<StudentsDD> res = new List<StudentsDD>();
            res = repo.GetStudentDropDown();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
        [HttpGet("GetInstructorDropDown")]
        public async Task<IActionResult> GetInstructorDropDown()
        {
            List<InstructorsDto> res = new List<InstructorsDto>();
            res = irepo.GetAllInstructorsDD();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
    }
}
