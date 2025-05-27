using LMS.BAL.Interfaces;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.UserDTOs;
using LMS.Components.Utilities;
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

        public CommonController(ICourseMgmtService _mservice, IUserMgmtService uService)
        {
            mService = _mservice;
            this.uService = uService;
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
    }
}
