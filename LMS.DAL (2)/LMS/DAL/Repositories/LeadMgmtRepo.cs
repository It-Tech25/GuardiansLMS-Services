﻿using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.Leads;
using LMS.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.EntityFrameworkCore;
using LMS.Components.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;


namespace LMS.DAL.Repositories
{
    public class LeadMgmtRepo : ILeadMgmtRepo
    {
        private readonly MyDbContext context;
        private readonly IConfiguration _configuration;

        public LeadMgmtRepo(MyDbContext _context, IConfiguration configuration)
        {
            context = _context;
            _configuration = configuration;

        }

        #region Lead Master Crud Operations
        public List<LeadMasterListDTO> GetLeadListByUser(string search = "",int uid=0)
        {
            List<LeadMasterListDTO> response = new List<LeadMasterListDTO>();
            try
            {
                StatusTypes leadstatus = context.statusTypes.Where(l => l.TypeName == "Leads" && l.IsDeleted == false).FirstOrDefault();
                response = (from l in context.leads
                            join s in context.commonStatuses on l.StatusId equals s.StatusId
                            //join c in context.courseTypes on l.IntrestedCourse equals c.CourseId
                            join u in context.userEntities on l.AssignedUserId equals u.UserId
                            where l.IsDeleted == false && l.Name.Contains(search) && l.AssignedUserId==uid
                            select new LeadMasterListDTO
                            {
                                LeadId = l.LeadId,
                                FromSource = l.FromSource,
                                MobileNumber = l.MobileNumber,
                                Email = l.Email,
                                Name = l.Name,
                                IntrestedCourse = l.InterestedCourse,
                                AssignedUser = u.UserName,
                                Status = s.StatusName,
                                CreatedOn = l.CreatedOn,
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public List<LeadMasterListDTO> GetLeadList(string search = "")
        {
            List<LeadMasterListDTO> response = new List<LeadMasterListDTO>();
            try
            {
                StatusTypes leadstatus = context.statusTypes.Where(l => l.TypeName == "LeadMaster" && l.IsDeleted == false).FirstOrDefault();
                response = (from l in context.leads
                            join s in context.commonStatuses on l.StatusId equals s.StatusId
                            //join c in context.courseTypes on l.IntrestedCourse equals c.CourseId
                            join u in context.userEntities on l.AssignedUserId equals u.UserId
                            where l.IsDeleted == false && l.Name.Contains(search) && s.StatusTypeId == leadstatus.TypeId
                            select new LeadMasterListDTO
                            {
                                LeadId = l.LeadId,
                                FromSource = l.FromSource,
                                MobileNumber = l.MobileNumber,
                                Email = l.Email,
                                Name = l.Name,
                                IntrestedCourse = l.InterestedCourse,
                                AssignedUser = u.UserName,
                                Status = s.StatusName,
                                CreatedOn = l.CreatedOn,
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }
        public List<LeadMasterListDTO> GetClossedList(string search = "")
        {
            List<LeadMasterListDTO> response = new List<LeadMasterListDTO>();
            try
            {
                CommonStatus leadstatus = context.commonStatuses.Where(l => l.StatusName == "Clossed" && l.IsDeleted == false).FirstOrDefault();
                response = (from l in context.leads
                            join s in context.commonStatuses on l.StatusId equals s.StatusId
                            //join c in context.courseTypes on l.IntrestedCourse equals c.CourseId
                            join u in context.userEntities on l.AssignedUserId equals u.UserId
                            where l.IsDeleted == false && l.Name.Contains(search) && s.StatusTypeId == leadstatus.StatusTypeId && l.StatusId == 20
                            select new LeadMasterListDTO
                            {
                                LeadId = l.LeadId,
                                FromSource = l.FromSource,
                                MobileNumber = l.MobileNumber,
                                Email = l.Email,
                                Name = l.Name,
                                IntrestedCourse = l.InterestedCourse,
                                AssignedUser = u.UserName,
                                Status = s.StatusName,
                                CreatedOn = l.CreatedOn,
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>> GetNegotiationList(LeadFilterDto filter)
        {
            var response = new ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>();
            try
            {
                var leadstatus = context.commonStatuses
                    .FirstOrDefault(l => l.StatusName == "Negotiation" && l.IsDeleted == false);

                if (leadstatus == null)
                {
                    response.Succeded = false;
                    response.Response = new List<LeadMasterQualifiedListDTO>();
                    return response;
                }

                if (leadstatus != null)
                {
                    var data = (from l in context.leads
                                join s in context.commonStatuses on l.StatusId equals s.StatusId
                                join u in context.userEntities on l.AssignedUserId equals u.UserId
                                where l.IsDeleted == false && l.StatusId == leadstatus.StatusId
                                select new LeadMasterQualifiedListDTO
                                {
                                    LeadId = l.LeadId,
                                    FromSource = l.FromSource,
                                    MobileNumber = l.MobileNumber,
                                    Email = l.Email,
                                    Name = l.Name,
                                    InterestedCourse = l.InterestedCourse,
                                    AssignedUser = u.UserName
                                }).ToList();
                    response.Succeded = true;
                    response.Response = data;
                }


            }
            catch (Exception ex)
            {
                response.Succeded = false;
                response.Response = new List<LeadMasterQualifiedListDTO>();
            }

            return response;
        }


        public EditLeadModel GetLeadById(int id)
        {
            EditLeadModel response = new EditLeadModel();
            try
            {
                response = (from l in context.leads
                                where l.IsDeleted == false && l.LeadId == id
                                select new EditLeadModel
                                {
                                    LeadId = l.LeadId,
                                    FromSource = l.FromSource,
                                    MobileNumber = l.MobileNumber,
                                    Email = l.Email,
                                    Name = l.Name,
                                    IntrestedCourse = l.InterestedCourse,
                                    AssignedUser = l.AssignedUserId,
                                    StatusId = l.StatusId,

                                    Notes = (from note in context.leadNoteEntity
                                             where note.LeadId == l.LeadId
                                             orderby note.CreatedOn descending
                                             select new NoteModel
                                             {
                                                 NoteText = note.NoteText,
                                                 CreatedOn = note.CreatedOn,
                                                 CreatedByUserName = context.userEntities
                                                     .Where(u => u.UserId == note.CreatedBy)
                                                     .Select(u => u.UserName)
                                                     .FirstOrDefault()
                                             }).ToList()
                                }).FirstOrDefault();

            }
            catch (Exception ex) { }
            return response;
        }

        public LeadMaster GetLeadModelById(int id)
        {
            LeadMaster response = new LeadMaster();
            try
            {
                response = context.leads.Where(m => m.LeadId == id && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public GenericResponse AddLead(LeadMaster req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.leads.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.LeadId;
                LeadAudit la = new LeadAudit()
                {
                    LeadId = req.LeadId,
                    EventType = "Creation",
                    EventDate = DateTime.Now,
                    EventDetails = "Created the lead with Name :- " + req.Name + ", email:- " + req.Email + ", number:- " + req.MobileNumber,
                    ChangedBy = req.CreatedBy,
                    IsDeleted = false
                };
                CreateLeadAudit(la);
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateLead(LeadMaster req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.leads.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.LeadId;
            }
            catch (Exception ex)
            {
                response.statusCode = 404;
                response.Message = ex.Message;
            }
            return response;
        }

        public void CreateLeadAudit(LeadAudit req)
        {
            try
            {
                context.leadAudits.Update(req);
                context.SaveChanges();
            }
            catch (Exception ex) { }
        }

        public async Task AddLeadAsync(LeadMaster lead)
        {
            context.leads.Add(lead);
            await context.SaveChangesAsync();
        }


        #endregion

        public LeadMaster GetLeadModelByNumber(string number)
        {
            LeadMaster response = new LeadMaster();
            try
            {
                response = context.leads.Where(m => m.MobileNumber == number && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }



        public GenericResponse UpdateAssignedUser(UpdateLeadStatusDto dto, int uuserId)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var lead = context.leads.Find(dto.LeadId);
                if (lead == null)
                {
                    response.statusCode = 404;
                    response.Message = "Lead not found";
                    return response;
                }

                lead.AssignedUserId = dto.userid;
                lead.ModifiedBy = uuserId;
                lead.ModifiedOn = DateTime.UtcNow;

                context.SaveChanges();

                response.statusCode = 200;
                response.Message = "Assigned user updated successfully";
                response.CurrentId = lead.LeadId;
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }



        public GenericResponse AddLeadNote(AddLeadNoteDto noteDto, int Userid)
        {
            GenericResponse response = new GenericResponse();
            try
            {
               
                
                    var note = new LeadNoteEntity
                    {
                        LeadId = noteDto.LeadId,
                        NoteText = noteDto.NoteText,
                        CreatedBy = Userid,
                        CreatedOn = DateTime.UtcNow
                    };

                    context.leadNoteEntity.Add(note);
                    context.SaveChanges();

                    response.statusCode = 200;
                    response.Message = "Note added successfully";
                    response.CurrentId = note.LeadId;
               
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }

        public GenericResponse UpdateLeadNote(AddLeadNoteDto noteDto)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var res = context.leadNoteEntity.Where(a => a.LeadId == noteDto.LeadId).FirstOrDefault();

                res.LeadId = noteDto.LeadId;
                res.NoteText = noteDto.NoteText;
                res.CreatedOn = DateTime.UtcNow;


                context.leadNoteEntity.Update(res);
                context.SaveChanges();

                response.statusCode = 200;
                response.Message = "Note Updated successfully";
                response.CurrentId = res.LeadId;
            }
            catch (Exception ex)
            {
                response.statusCode = 500;
                response.Message = $"Error: {ex.Message}";
            }
            return response;
        }


        public async Task<ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>> GetUnassignedLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
        new SqlParameter("@userId", currentUserId),
        new SqlParameter("@mode", "UnAssigned"),
        new SqlParameter("@stage", ""),
        new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
        new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
        new SqlParameter("@Source", filter.Source ?? ""),
        new SqlParameter("@Course", filter.Course ?? ""),
        new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
    };

            var finalList = await context.leadMasterUnAssignedListDTO
                .FromSqlRaw("EXEC dbo.GetLeadsList @userId, @mode, @stage, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();

            var result = finalList.Select(l => new LeadMasterUnAssignedListDTO
            {
                LeadId = l.LeadId,
                FromSource = l.FromSource,
                MobileNumber = l.MobileNumber,
                Email = l.Email,
                Name = l.Name,
                InterestedCourse = l.InterestedCourse,
                IsShowReAsign = l.IsShowReAsign,
                // AssignedUser = l.AssignedUser
            });

            var response = new ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>
            {
                Response = result,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };

            return response;
        }



        public List<AllLeadMasterListDTO> AllGetLeads(LeadFilterDto filter, int currentUserId)
        {
            //        var parameters = new[]
            //        {
            //    new SqlParameter("@userId", currentUserId),
            //    new SqlParameter("@mode", "All"),
            //    new SqlParameter("@stage", ""),
            //    new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
            //    new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
            //    new SqlParameter("@Source", filter.Source ?? ""),
            //    new SqlParameter("@Course", filter.Course ?? ""),
            //    new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
            //};

            //        var finalList = context.allLeadMasterListDTO
            //            .FromSqlRaw("EXEC dbo.GetLeadsList @userId, @mode, @stage, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
            //            .ToList();

            //        // If projection is needed:
            //        var mappedResult = finalList.Select(l => new AllLeadMasterListDTO
            //        {
            //            LeadId = l.LeadId,
            //            FromSource = l.FromSource,
            //            MobileNumber = l.MobileNumber,
            //            Email = l.Email,
            //            Name = l.Name,
            //            InterestedCourse = l.InterestedCourse,
            //            IsShowReAsign = l.IsShowReAsign,
            //            AssignedUser = l.AssignedUser
            //        }).ToList(); // ✅ convert to List here

            //        return mappedResult; // ✅ returning List<AllLeadMasterListDTO>

            var finalList = context.leads.Where(a => a.IsDeleted == false).ToList();
            var mappedResult = finalList.Select(l => new AllLeadMasterListDTO
            {
                LeadId = l.LeadId,
                FromSource = l.FromSource,
                MobileNumber = l.MobileNumber,
                Email = l.Email,
                Name = l.Name,
                InterestedCourse = l.InterestedCourse,
                AssignedUser =context.userEntities.Where(a=>a.UserId==l.AssignedUserId).Select(a=>a.UserName).FirstOrDefault()
            }).ToList(); // ✅ convert to List here

            return mappedResult;

        }





        public async Task<ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>> GetAssignedLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
        new SqlParameter("@userId", currentUserId),
        new SqlParameter("@mode", "Assigned"),
        new SqlParameter("@status", ""),
        new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
        new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
        new SqlParameter("@Source", filter.Source ?? ""),
        new SqlParameter("@Course", filter.Course ?? ""),
        new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
    };

            var leads = await context.leadMasterAssignedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();

            var result = leads.Select(l => new LeadMasterAssignedListDTO
            {
                LeadId = l.LeadId,
                FromSource = l.FromSource,
                MobileNumber = l.MobileNumber,
                Email = l.Email,
                Name = l.Name,
                InterestedCourse = l.InterestedCourse,
                IsShowReAsign = l.IsShowReAsign,
                AssignedUser = l.AssignedUser
            });


            var response = new ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>
            {
                Response = result,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };

            return response;
        }




        public async Task<ApiResponse<IEnumerable<LeadMasterContactedListDTO>>> GetContactedLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
    new SqlParameter("@userId", currentUserId),
    new SqlParameter("@mode", "Contacted"),
   new SqlParameter("@status", ""),
    new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
    new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
    new SqlParameter("@Source", filter.Source ?? ""),
    new SqlParameter("@Course", filter.Course ?? ""),
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
      };


            var leads = await context.leadMasterContactedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();

            var result = leads.Select(l => new LeadMasterContactedListDTO
            {
                LeadId = l.LeadId,
                FromSource = l.FromSource,
                MobileNumber = l.MobileNumber,
                Email = l.Email,
                Name = l.Name,
                InterestedCourse = l.InterestedCourse,
                CreatedOn = l.CreatedOn,
                IsShowReAsign = l.IsShowReAsign,
                AssignedUser = l.AssignedUser
            });

            var response = new ApiResponse<IEnumerable<LeadMasterContactedListDTO>>
            {
                Response = result,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };



            return response;


        }


        public async Task<ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>> GetQualifiedLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
    new SqlParameter("@userId", currentUserId),
    new SqlParameter("@mode", "Qualified"),
    new SqlParameter("@status",  ""),
    new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
    new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
    new SqlParameter("@Source", filter.Source ?? ""),
    new SqlParameter("@Course", filter.Course ?? ""),
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
};

            var leads = await context.leadMasterQualifiedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();


            var response = new ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>
            {
                Response = leads,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };



            return response;
        }


        public async Task<ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>> GetFollowUpLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
    new SqlParameter("@userId", currentUserId),
    new SqlParameter("@mode", "Follow-up"),
   new SqlParameter("@status", ""),
    new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
    new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
    new SqlParameter("@Source", filter.Source ?? ""),
    new SqlParameter("@Course", filter.Course ?? ""),
    new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
};

            var leads = await context.leadMasterFollowupListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();



            var response = new ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>
            {
                Response = leads,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };



            return response;
        }


        public async Task<ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>> GetCounsellingDoneLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
    new SqlParameter("@userId", currentUserId),
    new SqlParameter("@mode", "Counselling Done"),
 new SqlParameter("@status",  ""),
    new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
    new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
    new SqlParameter("@Source", filter.Source ?? ""),
    new SqlParameter("@Course", filter.Course ?? ""),
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }
};

            var leads = await context.leadMasterCounsellingListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo", parameters)
                .ToListAsync();

            var response = new ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>
            {
                Response = leads,
                Succeded = true,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };



            return response;
        }




        //    private static readonly Dictionary<string, List<string>> StatusTransitions = new Dictionary<string, List<string>>
        //{
        //    { "UnAssigned", new List<string> { "Assigned","Contacted", "Follow-up", "Counselling Done", "Negotiation" } },
        //    { "Assigned", new List<string> { "Contacted", "Follow-up", "Counselling Done", "Negotiation" } },
        //    { "Contacted", new List<string> { "Qualified", "Follow-up", "Counselling Done", "Negotiation" } },
        //    { "Qualified", new List<string> { "Follow-up", "Counselling Done", "Negotiation" } },
        //    { "Follow-up", new List<string> { "Counselling Done", "Negotiation" } },
        //    { "Counselling Done", new List<string> { "Negotiation" } },
        //    { "Negotiation", new List<string> { "Closed" } }
        //    // Add other transitions as needed
        //};


        //public async Task<Dictionary<string, List<string>>> GetStatusTransitionsAsync()
        //{
        //    var statuses = await context.commonStatuses
        //        .Where(s => s.IsDeleted == false)
        //        .OrderBy(s => s.StatusId) // Assuming StatusId defines the order
        //        .Select(s => s.StatusName)
        //        .ToListAsync();

        //    var statusTransitions = new Dictionary<string, List<string>>();

        //    for (int i = 0; i < statuses.Count; i++)
        //    {
        //        var currentStatus = statuses[i];
        //        var nextStatuses = statuses.Skip(i + 1).ToList();
        //        statusTransitions[currentStatus] = nextStatuses;
        //    }

        //    return statusTransitions;
        //}




        //public async Task<ApiResponse<object>> GetNextStatusesAsync(string currentStatus)
        //{
        //    var response = new ApiResponse<object>();

        //    if (string.IsNullOrEmpty(currentStatus))
        //    {
        //        response.Succeded = false;
        //        response.Errors = new List<string> { "Current status is required." };
        //        return response;
        //    }

        //    if (!StatusTransitions.TryGetValue(currentStatus, out var nextStatuses))
        //    {
        //        response.Succeded = false;
        //        response.Errors = new List<string> { $"No transitions found for status: {currentStatus}" };
        //        return response;
        //    }


        //    var salesUsers = await context.userEntities
        //        .Where(u => u.UserType == 5)
        //        .Select(u => new { u.UserId, u.UserName })
        //        .ToListAsync();

        //    response.Succeded = true;
        //    response.Response = new
        //    {
        //        CurrentStatus = currentStatus,
        //        NextStatuses = nextStatuses,
        //        SalesUsers = salesUsers
        //    };

        //    return response;
        //}


        public async Task<ApiResponse<object>> GetNextStatusesAsync(string currentStatus)
        {
            var response = new ApiResponse<object>();

            if (string.IsNullOrEmpty(currentStatus))
            {
                response.Succeded = false;
                response.Errors = new List<string> { "Current status is required." };
                return response;
            }

            // Retrieve all active statuses ordered by StatusId
            var statuses = await context.commonStatuses
                .Where(s => s.IsDeleted == false)
                .OrderBy(s => s.StatusId)
                .Select(s => s.StatusName)
                .ToListAsync();

            // Build the StatusTransitions dictionary dynamically
            var statusTransitions = new Dictionary<string, List<string>>();
            for (int i = 0; i < statuses.Count; i++)
            {
                var current = statuses[i];
                var nextStatuses = statuses.Skip(i + 1).ToList();
                statusTransitions[current] = nextStatuses;
            }

            if (!statusTransitions.TryGetValue(currentStatus, out var nextStatusesList))
            {
                response.Succeded = false;
                response.Errors = new List<string> { $"No transitions found for status: {currentStatus}" };
                return response;
            }

            // Retrieve sales users (UserType == 5)
            var salesUsers = await context.userEntities
                .Where(u => u.UserType == 5)
                .Select(u => new { u.UserId, u.UserName })
                .ToListAsync();

            response.Succeded = true;
            response.Response = new
            {
                CurrentStatus = currentStatus,
                NextStatuses = nextStatusesList,
                SalesUsers = salesUsers
            };

            return response;
        }



        public async Task<LeadMaster> GetLeadByIdAsync(int leadId)
        {
            return await context.leads
                .FirstOrDefaultAsync(l => l.LeadId == leadId && l.IsDeleted == false);
        }

        public async Task<CommonStatus> GetStatusByNameAsync(string statusName)
        {
            return await context.commonStatuses
                .FirstOrDefaultAsync(s => s.StatusName == statusName && s.IsDeleted == false);
        }

        public async Task<bool> UpdateLeadAsync(LeadMaster lead)
        {
            context.leads.Update(lead);
            return await context.SaveChangesAsync() > 0;
        }


    }
}
