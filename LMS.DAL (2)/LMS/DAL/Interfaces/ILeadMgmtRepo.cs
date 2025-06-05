using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Leads;
using LMS.Components.Utilities;

namespace LMS.DAL.Interfaces
{
    public interface ILeadMgmtRepo
    {
        #region Lead Master Crud Operations

        List<LeadMasterListDTO> GetLeadList(string search = "");
        EditLeadModel GetLeadById(int id);
        LeadMaster GetLeadModelById(int id);
        GenericResponse AddLead(LeadMaster req);
        GenericResponse UpdateLead(LeadMaster req);
        void CreateLeadAudit(LeadAudit req);

        #endregion

        LeadMaster GetLeadModelByNumber(string number);

       // Task<IEnumerable<AllLeadMasterListDTO>> AllGetLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<AllLeadMasterListDTO>>> AllGetLeads(LeadFilterDto filter, int currentUserId);
         Task<ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>> GetUnassignedLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>> GetAssignedLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterContactedListDTO>> >GetContactedLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>> GetQualifiedLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>> GetFollowUpLeads(LeadFilterDto filter, int currentUserId);
        Task<ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>> GetCounsellingDoneLeads(LeadFilterDto filter, int currentUserId);
        List<LeadMasterListDTO> GetClossedList(string search = "");
        GenericResponse AddLeadNote(AddLeadNoteDto noteDto);
        GenericResponse UpdateAssignedUser(UpdateLeadStatusDto dto, int uuserId);

        Task<ApiResponse<object>> GetNextStatusesAsync(string currentStatus);
        Task<LeadMaster> GetLeadByIdAsync(int leadId);
        Task<CommonStatus> GetStatusByNameAsync(string statusName);
        Task<bool> UpdateLeadAsync(LeadMaster lead);
    }
}
