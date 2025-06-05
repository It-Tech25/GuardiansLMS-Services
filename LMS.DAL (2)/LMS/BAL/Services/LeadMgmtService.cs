using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Leads;
using LMS.Components.Utilities;
using LMS.DAL.Interfaces;
using LMS.DAL.Repositories;

namespace LMS.BAL.Services
{
    public class LeadMgmtService : ILeadMgmtService
    {
        private readonly ILeadMgmtRepo lRepo;
        private readonly ICommonDDRepo commonRepo;

        public LeadMgmtService(ILeadMgmtRepo mRepo, ICommonDDRepo _commonRepo)
        {
            lRepo = mRepo;
            commonRepo = _commonRepo;
        }

        #region Lead Master Crud Operations

        public List<LeadMasterListDTO> GetLeadList(string search = "")
        {
            return lRepo.GetLeadList(search);
        }

        public EditLeadModel GetLeadById(int id)
        {
            return lRepo.GetLeadById(id);
        }  

        public GenericResponse AddEditLead(AddLeadModel req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            LeadMaster lm = lRepo.GetLeadModelByNumber(req.MobileNumber);

            StatusTypes st = commonRepo.GetStatusTypeByName("Leads");
            CommonStatus cs = new CommonStatus();
            if (st != null)
            {
                if (req.AssignedUser > 0)
                {
                    cs = commonRepo.GetStatusByName("Assigned", st.TypeId);
                }
                else
                {
                    cs = commonRepo.GetStatusByName("Unassigned", st.TypeId);
                }
            }

            if (req.LeadId == 0)
            {
                if (lm == null)
                {
                    lm = new LeadMaster();
                    lm.Name = req.Name;
                    lm.MobileNumber = req.MobileNumber;
                    lm.FromSource = req.FromSource;
                    lm.Email = req.Email;
                    lm.InterestedCourse = req.IntrestedCourse;
                    lm.AssignedUserId = req.AssignedUser > 0 ? req.AssignedUser : null;
                    lm.CreatedOn = DateTime.Now;
                    lm.CreatedBy = currentUserId;
                    lm.IsActive = true;
                    lm.IsDeleted = false;
                    lm.StatusId = cs.StatusId;
                    lm.AssignedOn = cs.StatusName == "Assigned" ? DateTime.Now : null;

                    res = lRepo.AddLead(lm);
                }
                else
                {
                    res.statusCode = 0;
                    res.Message = "Number is already used";
                }
            }
            else
            {
                bool isDuplicate = false;
                if(lm != null)
                {
                    if(req.LeadId != lm.LeadId)
                    {
                        isDuplicate = true;
                    }
                }
                if (isDuplicate)
                {
                    res.statusCode = 0;
                    res.Message = "Number is already used";
                }
                else
                {
                    lm = lRepo.GetLeadModelById(req.LeadId);
                    if (lm != null)
                    {
                        lm.Name = req.Name;
                        lm.MobileNumber = req.MobileNumber;
                        lm.Email = req.Email;
                        lm.InterestedCourse = req.IntrestedCourse;
                        lm.AssignedUserId = req.AssignedUser;
                        lm.ModifiedOn = DateTime.Now;
                        lm.ModifiedBy = currentUserId;
                        lm.IsActive = true;
                        lm.IsDeleted = false;
                        lm.StatusId = cs.StatusId;
                        lm.AssignedOn = cs.StatusName == "Assigned" ? DateTime.Now : null;

                        res = lRepo.UpdateLead(lm);
                        LeadAudit l = new LeadAudit();
                        l.LeadId = req.LeadId;
                        l.IsDeleted = false;
                        l.EventDate = DateTime.Now;
                        l.ChangedBy = currentUserId;
                        if (lm.Name != req.Name)
                        {
                            l.EventType = "Update";
                            l.EventDetails = "Updated Name";
                            l.OldValue = lm.Name;
                            l.NewValue = req.Name;
                            l.ChangedField = "Name";
                            lRepo.CreateLeadAudit(l);
                        }
                        if(lm.MobileNumber != req.MobileNumber)
                        {
                            l.EventType = "Update";
                            l.EventDetails = "Updated MobileNumber";
                            l.OldValue = lm.MobileNumber;
                            l.NewValue = req.MobileNumber;
                            l.ChangedField = "MobileNumber";
                            lRepo.CreateLeadAudit(l);
                        }
                        if(lm.Email != req.Email)
                        {
                            l.EventType = "Update";
                            l.EventDetails = "Updated Email";
                            l.OldValue = lm.Email;
                            l.NewValue = req.Email;
                            l.ChangedField = "Email";
                            lRepo.CreateLeadAudit(l);
                        }
                        if(lm.InterestedCourse != req.IntrestedCourse)
                        {
                            l.EventType = "Update";
                            l.EventDetails = "Updated IntrestedCourse";
                            l.OldValue = lm.InterestedCourse;
                            l.NewValue = req.IntrestedCourse;
                            l.ChangedField = "IntrestedCourse";
                            lRepo.CreateLeadAudit(l);
                        }
                    }
                    else
                    {
                        res.statusCode = 0;
                        res.Message = "Id does not exists";
                    }
                }
            }

            return res;
        }
        #endregion
        
       

        public GenericResponse AddLeadNote(AddLeadNoteDto noteDto)
        {
            return  lRepo.AddLeadNote(noteDto);
        }
        public GenericResponse UpdateAssignedUser(UpdateLeadStatusDto dto, int uuserId)
        {
            return  lRepo.UpdateAssignedUser(dto, uuserId);
        } 
        public Task<ApiResponse<object>> GetNextStatusesAsync(string currentStatus)
        {
            return  lRepo.GetNextStatusesAsync(currentStatus);
        }
       public Task<LeadMaster> GetLeadByIdAsync(int leadId)
        {
            return lRepo.GetLeadByIdAsync(leadId);
        }
        public Task<CommonStatus> GetStatusByNameAsync(string statusName)
        {
            return lRepo.GetStatusByNameAsync(statusName);
        }
        public Task<bool> UpdateLeadAsync(LeadMaster lead)
        {
            return lRepo.UpdateLeadAsync(lead);
        }
      public  List<LeadMasterListDTO> GetClossedList(string search = "")
        {
            return lRepo.GetClossedList(search);
        }
        public async Task<ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>> GetUnassignedLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetUnassignedLeads(filter, currentUserId);
        }

        public async Task<ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>> GetAssignedLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetAssignedLeads(filter, currentUserId);
        }

        public async Task<ApiResponse<IEnumerable<LeadMasterContactedListDTO>>> GetContactedLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetContactedLeads(filter, currentUserId);
        }

        public async Task<ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>> GetQualifiedLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetQualifiedLeads(filter, currentUserId);
        }

        public async Task<ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>> GetFollowUpLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetFollowUpLeads(filter, currentUserId);
        }

        public async Task<ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>> GetCounsellingDoneLeadsAsync(LeadFilterDto filter, int currentUserId)
        {
            return await lRepo.GetCounsellingDoneLeads(filter, currentUserId);
        }


        //public Task<IEnumerable<AllLeadMasterListDTO>> AllGetLeads(LeadFilterDto filter, int currentUserId)
        //{
        //    return lRepo.AllGetLeads(filter, currentUserId);
        //} 

        public Task<ApiResponse<IEnumerable<AllLeadMasterListDTO>>> AllGetLeads(LeadFilterDto filter, int currentUserId)
        {
            return lRepo.AllGetLeads(filter, currentUserId);
        }


    }
}
