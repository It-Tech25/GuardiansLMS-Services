using LMS.BAL.Interfaces;
using LMS.BAL.Services;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.CourseType;
using LMS.Components.ModelClasses.Leads;
using LMS.Components.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        public IActionResult GetAllLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            // Assuming your service now returns a List<AllLeadMasterListDTO>
            var list = lService.AllGetLeads(filter, userId);

            var filtered = list.AsQueryable();

            var skip = (pagenumber - 1) * pagesize;
            var paged = filtered.Skip(skip).Take(pagesize).ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = filtered.Count(),
                data = paged
            });
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
        public async Task<IActionResult> GetUnassignedLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>> res = await lService.GetUnassignedLeadsAsync(filter, userId);

            if (res?.Response == null)
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterUnAssignedListDTO>()
                });
            }

            var data = res.Response.ToList(); 
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }




        [HttpGet("AssignedLeads")]
        public async Task<IActionResult> GetAssignedLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterAssignedListDTO>> res = await lService.GetAssignedLeadsAsync(filter, userId);

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterAssignedListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }

        [HttpGet("ContactedLeads")]
        public async Task<IActionResult> GetContactedLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterContactedListDTO>> res = await lService.GetContactedLeadsAsync(filter, userId);

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterContactedListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }

        [HttpGet("QualifiedLeads")]
        public async Task<IActionResult> GetQualifiedLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>> res = await lService.GetQualifiedLeadsAsync(filter, userId);

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterQualifiedListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }


        [HttpGet("FollowUpLeads")]
        public async Task<IActionResult> GetFollowUpLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));
            ApiResponse<IEnumerable<LeadMasterFollowupListDTO>> res = await lService.GetFollowUpLeadsAsync(filter, userId);

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterFollowupListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }

        [HttpGet("CounsellingDoneLeads")]
        public async Task<IActionResult> GetCounsellingDoneLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>> res = await lService.GetCounsellingDoneLeadsAsync(filter, userId);

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterCounsellingListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
        }

        [HttpGet("NegotiationLeads")]

        public async Task<IActionResult> GetNegotiationLeads([FromQuery] LeadFilterDto filter, int pagenumber = 1, int pagesize = 10)
        {
            var userId = int.Parse(User.FindFirstValue("UserID"));

            var res = await lService.GetNegotiationList(filter); 

            if (res?.Response == null || !res.Response.Any())
            {
                return Ok(new
                {
                    succeeded = false,
                    message = "No data found",
                    totalRecords = 0,
                    data = new List<LeadMasterQualifiedListDTO>()
                });
            }

            var data = res.Response.ToList();
            var totalRecords = data.Count;

            var pagedData = data
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .ToList();

            return Ok(new
            {
                succeeded = true,
                totalRecords = totalRecords,
                data = pagedData
            });
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
           
            var result = lService.AddLeadNote(noteDto,userId);

            if (result.statusCode == 200)
                return Ok(result);
            else
                return StatusCode(result.statusCode, result);
        }
        //[HttpPost("UpdateLeadNote")]
        //public IActionResult UpdateLeadNote([FromBody] AddLeadNoteDto noteDto)
        //{
        //    var userId = int.Parse(User.FindFirstValue("UserID"));

        //    var result = lService.UpdateLeadNote(noteDto);

        //    if (result.statusCode == 200)
        //        return Ok(result);
        //    else
        //        return StatusCode(result.statusCode, result);
        //}



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
