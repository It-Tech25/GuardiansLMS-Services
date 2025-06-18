using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Leads;
using LMS.Components.Utilities;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BAL.Interfaces
{
    public interface ILeadMgmtService
    {
        List<LeadMasterListDTO> GetLeadList(string search = "");
        List<LeadMasterListDTO> GetLeadListByUser(string search = "", int uid = 0);
        EditLeadModel GetLeadById(int id);
        Task<ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>> GetNegotiationList(LeadFilterDto filter);
        GenericResponse AddEditLead(AddLeadModel req, int currentUserId);
        // Task<IEnumerable<AllLeadMasterListDTO>> AllGetLeads(LeadFilterDto filter, int currentUserId);
        List<AllLeadMasterListDTO> AllGetLeads(LeadFilterDto filter, int currentUserId);
        Task<string> ProcessBulkLeadsAsync(IFormFile file,int UserId);
        Task<ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>> GetUnassignedLeadsAsync(LeadFilterDto filter,int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>> GetAssignedLeadsAsync(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterContactedListDTO>>> GetContactedLeadsAsync(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>> GetQualifiedLeadsAsync(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>> GetFollowUpLeadsAsync(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>> GetCounsellingDoneLeadsAsync(LeadFilterDto filter, int currentUserId);
        List<LeadMasterListDTO> GetClossedList(string search = "");
        GenericResponse AddLeadNote(AddLeadNoteDto noteDto,int Userid);
        GenericResponse UpdateLeadNote(AddLeadNoteDto noteDto);
        GenericResponse UpdateAssignedUser(UpdateLeadStatusDto dto, int uuserId);
        Task<ApiResponse<object>> GetNextStatusesAsync(string currentStatus);
        Task<LeadMaster> GetLeadByIdAsync(int leadId);
        Task<CommonStatus> GetStatusByNameAsync(string statusName);
        Task<bool> UpdateLeadAsync(LeadMaster lead);

    }
}
