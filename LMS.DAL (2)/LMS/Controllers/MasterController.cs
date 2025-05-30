using LMS.BAL.Interfaces;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.Components.Utilities;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController : ControllerBase
    {
        private readonly IMasterMgmtService mService;

        public MasterController(IMasterMgmtService _mservice)
        {
            mService = _mservice;
        }

        #region UserTypes

        [HttpGet("GetUserTypeList")]
        public async Task<IActionResult> GetUserTypeList(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            List<UserTypeDTO> res = mService.GetUserTypeList(searchterm);

            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = res.Skip(skip).Take(pagesize).ToList();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(pagedResult);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();  

            return Ok(finalResponse);
        }


        [HttpGet("GetUserTypesDD")]
        public async Task<IActionResult> GetUserTypesDD()
        {
            List<UserTypeDD> res = new List<UserTypeDD>();
            res = mService.GetUserTypesDD();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }

        [HttpGet("GetUserTypeById")]
        public async Task<IActionResult> GetUserTypeById(int id)
        {
            UserTypeDTO res = new UserTypeDTO();
            res = mService.GetUserTypeById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPost("AddEditUserType")]
        public async Task<IActionResult> AddEditUserType(UserTypeDTO req)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.AddEditUserType(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("ActivateUserType")]
        public async Task<IActionResult> ActivateUserType(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.ActivateUserType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeActivateUserType")]
        public async Task<IActionResult> DeActivateUserType(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeActivateUserType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeleteUserType")]
        public async Task<IActionResult> DeleteUserType(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeleteUserType(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        #endregion
    }
}
