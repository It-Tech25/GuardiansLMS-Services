using Azure;
using LMS.BAL.Interfaces;
using LMS.BAL.Services;
using LMS.Components.ModelClasses;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Login;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.Components.Utilities;
using LMS.Models.ModelClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleMgmtService mService;

        public ModuleController(IModuleMgmtService _mservice)
        {
            mService = _mservice;
        }

        #region Module

        [HttpGet("GetModulesList")]
        public async Task<IActionResult> GetModulesList(string searchterm = "", int pagenumber = 0, int pagesize = 0)
        {
            // Step 1: Get the full filtered list from the service
            List<ModuleDTO> res = mService.GetModulesList(searchterm);

            // Step 2: Apply pagination
            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = res.Skip(skip).Take(pagesize).ToList();

            // Step 3: Build the API response
            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(pagedResult);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count(); // total before pagination

            return Ok(finalResponse);
        }


        [HttpGet("GetModulesDropDown")]
        public async Task<IActionResult> GetModulesDropDown()
        {
            List<ModuleDD> res = new List<ModuleDD>();
            res = mService.GetModulesDD();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }
        
        [HttpGet("GetModuleById")]
        public async Task<IActionResult> GetModuleById(int id)
        {
            ModuleDTO res = new ModuleDTO();
            res = mService.GetModuleById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPost("AddEditModule")]
        public async Task<IActionResult> AddEditModule(ModuleAddDTO req)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.AddEditModule(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("ActivateModule")]
        public async Task<IActionResult> ActivateModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.ActivateModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeActivateModule")]
        public async Task<IActionResult> DeActivateModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeActivateModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeleteModule")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeleteModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        #endregion

        #region Sub Module

        [HttpGet("GetSubModulesList")]
        public async Task<IActionResult> GetSubModulesList(string searchterm = "", int pagenumber = 0, int pagesize = 0, int moduleId=0)
        {
            List<SubModuleListDTO> res = mService.GetSubModulesList(moduleId,searchterm);

            int skip = (pagenumber - 1) * pagesize;
            var pagedResult = res.Skip(skip).Take(pagesize).ToList();

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(pagedResult);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count(); 

            return Ok(finalResponse);
        }


        [HttpGet("GetSubModulesDropDown")]
        public async Task<IActionResult> GetSubModulesDropDown(int moduleId)
        {
            List<SubModuleDD> res = new List<SubModuleDD>();
            res = mService.GetSubModulesDD(moduleId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;
            finalResponse.totalRecords = res.Count();

            return Ok(finalResponse);
        }

        [HttpGet("GetSubModuleById")]
        public async Task<IActionResult> GetSubModuleById(int id)
        {
            SubModuleDTO res = new SubModuleDTO();
            res = mService.GetSubModuleById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPost("AddEditSubModule")]
        public async Task<IActionResult> AddEditSubModule(SubModuleDTO req)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.AddEditSubModule(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("ActivateSubModule")]
        public async Task<IActionResult> ActivateSubModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.ActivateModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeActivateSubModule")]
        public async Task<IActionResult> DeActivateSubModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeActivateModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        [HttpPut("DeleteSubModule")]
        public async Task<IActionResult> DeleteSubModule(int id)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.DeleteModule(id, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }

        #endregion

        #region RoleMappping

        [HttpGet("GetUserRoleMappingData")]
        public async Task<IActionResult> GetUserRoleMappingData(int userTypeId)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            RoleMappingDTO res = new RoleMappingDTO();
            res = mService.GetUserRoleMappingData(userTypeId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }
        
        [HttpPost("UpdateRoleMappping")]
        public async Task<IActionResult> UpdateRoleMappping(RoleMappingDTO data)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));
            GenericResponse res = new GenericResponse();
            res = mService.UpdateRoleMappping(data, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }
        #endregion
    }
}
