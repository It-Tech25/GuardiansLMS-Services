using LMS.BAL.Interfaces;
using LMS.BAL.Services;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.Leads;
using LMS.Components.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LeadController : ControllerBase
    {
        private readonly ILeadMgmtService lService;

        public LeadController(ILeadMgmtService lService)
        {
            this.lService = lService;
        }


        [HttpPost("AddEditLead")]
        public async Task<IActionResult> AddEditLead(AddLeadModel req)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            GenericResponse res = new GenericResponse();
            res = lService.AddEditLead(req, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }


        [HttpGet("GetLeadById")]
        public async Task<IActionResult> GetLeadById(int id)
        {
            EditLeadModel res = new EditLeadModel();
            res = lService.GetLeadById(id);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            finalResponse.Succeded = true;

            return Ok(finalResponse);
        }



        //[HttpGet("GetLeadList")]
        //public async Task<IActionResult> GetLeadList(string search = "")
        //{
        //    List<LeadMasterListDTO> res = new List<LeadMasterListDTO>();
        //    res = lService.GetLeadList(search);

        //    var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
        //    finalResponse.Succeded = true;
        //    finalResponse.totalRecords = res.Count();

        //    return Ok(finalResponse);
        //}


         

        [HttpGet("GetAllLeads")]
        public async Task<IActionResult> GetAllLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            // Adjust the type of 'res' to match the return type of 'AllGetLeads'
            ApiResponse<IEnumerable<AllLeadMasterListDTO>> res = await lService.AllGetLeads(filter, userId);

            // Assuming ConvertResultToApiResonse processes the response appropriately
            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
        }

        [HttpGet("GetAllClossedLeads")]
        public IActionResult GetAllClossedLeads(string searchterm = "", int pagenumber = 1, int pagesize = 10)
        {
            var list = lService.GetClossedList(searchterm ?? "");

            var filtered = list.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchterm))
            {
                var term = searchterm.Trim().ToLower();
                filtered = filtered.Where(x =>
                    (!string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(term)) ||
                    (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(term)) ||
                    (!string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.ToLower().Contains(term))
                );
            }

            var skip = (pagenumber - 1) * pagesize;
            var paged = filtered.Skip(skip).Take(pagesize).ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = filtered.Count(),
                data = paged
            });
        }



        [HttpGet("UnassignedLeads")]
        public async Task<IActionResult> GetUnassignedLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

             ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>> res = await lService.GetUnassignedLeadsAsync(filter, userId);

             var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
        }




        [HttpGet("AssignedLeads")]
        public async Task<IActionResult> GetAssignedLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterAssignedListDTO>> res = await lService.GetAssignedLeadsAsync(filter, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
        }

        [HttpGet("ContactedLeads")]
        public async Task<IActionResult> GetContactedLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            ApiResponse<IEnumerable<LeadMasterContactedListDTO>> res = await lService.GetContactedLeadsAsync(filter, userId);

            var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
 
        }

        [HttpGet("QualifiedLeads")]
        public async Task<IActionResult> GetQualifiedLeads([FromQuery] LeadFilterDto filter)
        {
             var userId = int.Parse(User.FindFirstValue("UserID"));
            ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>> res = await lService.GetQualifiedLeadsAsync(filter, userId);

            // var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);

 
        }

        [HttpGet("FollowUpLeads")]
        public async Task<IActionResult> GetFollowUpLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            ApiResponse<IEnumerable<LeadMasterFollowupListDTO>> res = await lService.GetFollowUpLeadsAsync(filter, userId);

            //var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
 
        }

        [HttpGet("CounsellingDoneLeads")]
        public async Task<IActionResult> GetCounsellingDoneLeads([FromQuery] LeadFilterDto filter)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>> res = await lService.GetCounsellingDoneLeadsAsync(filter, userId);

            //var finalResponse = ConvertToAPI.ConvertResultToApiResonse(res);
            res.Succeded = true;

            return Ok(res);
             
        }

        [HttpPost("UpdateAssignedUser")]
        public IActionResult UpdateAssignedUser([FromBody] UpdateLeadStatusDto dto)
        {
            var uuserId = int.Parse(User.FindFirstValue("UserID"));

            var result = lService.UpdateAssignedUser(dto, uuserId);

            if (result.statusCode == 200)
                return Ok(result);
            else
                return StatusCode(result.statusCode, result);
        }

        [HttpPost("AddLeadNote")]
        public IActionResult AddLeadNote([FromBody] AddLeadNoteDto noteDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            var result = lService.AddLeadNote(noteDto);

            if (result.statusCode == 200)
                return Ok(result);
            else
                return StatusCode(result.statusCode, result);
        }



        [HttpGet("api/status/next")]
        public async Task<IActionResult> GetNextStatuses([FromQuery] string currentStatus)
        {
            var response = await lService.GetNextStatusesAsync(currentStatus);

            if (!response.Succeded)
            {
                if (response.Errors != null && response.Errors.Any())
                {
                    if (response.Errors.Contains("Current status is required."))
                        return BadRequest(response);
                    if (response.Errors.Contains("No transitions found"))
                        return NotFound(response);
                }

                return StatusCode(500, response);
            }

            return Ok(response);
        }



        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateLeadStatus([FromBody] LeadStatusUpdateRequest request)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            var response = new ApiResponse<object>();

            if (request == null || request.LeadId <= 0 || string.IsNullOrEmpty(request.NextStatus) || request.AssignedUserId <= 0)
            {
                response.Succeded = false;
                response.Errors = new List<string> { "Invalid input data." };
                return BadRequest(response);
            }

            var lead = await lService.GetLeadByIdAsync(request.LeadId);
            if (lead == null)
            {
                response.Succeded = false;
                response.Errors = new List<string> { "Lead not found." };
                return NotFound(response);
            }

            var status = await lService.GetStatusByNameAsync(request.NextStatus);
            if (status == null)
            {
                response.Succeded = false;
                response.Errors = new List<string> { "Status not found in CommonStatus." };
                return NotFound(response);
            }

            lead.AssignedUserId = request.AssignedUserId;
            lead.StatusId = status.StatusId;
            lead.ModifiedOn = DateTime.Now;
            lead.ModifiedBy = userId;

            var updateResult = await lService.UpdateLeadAsync(lead);

            if (!updateResult)
            {
                response.Succeded = false;
                response.Errors = new List<string> { "Failed to update lead." };
                return StatusCode(500, response);
            }

            response.Succeded = true;
            response.Response = new
            {
                Message = "Lead updated successfully.",
                LeadId = lead.LeadId,
                NewStatus = request.NextStatus,
                AssignedUserId = request.AssignedUserId
            };

            return Ok(response);
        }




    }
}
