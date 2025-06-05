using LMS.Components.Entities;
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
                StatusTypes leadstatus = context.statusTypes.Where(l => l.TypeName == "Clossed" && l.IsDeleted == false).FirstOrDefault();
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
                                StatusId = l.StatusId
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



        public GenericResponse AddLeadNote(AddLeadNoteDto noteDto)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var note = new LeadNoteEntity
                {
                    LeadId = noteDto.LeadId,
                    NoteText = noteDto.NoteText,
                  //  CreatedBy = noteDto.CreatedBy,
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
        new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 },
        new SqlParameter("@pageSize", filter.PageSize),
        new SqlParameter("@pageNumber", filter.PageNumber)
    };

            var finalList = await context.leadMasterUnAssignedListDTO
                .FromSqlRaw("EXEC dbo.GetLeadsList @userId, @mode, @stage, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
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

            var totalRecords = finalList.Count; // Ideally, fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + finalList.Count - 1;

            var response = new ApiResponse<IEnumerable<LeadMasterUnAssignedListDTO>>
            {
                Response = result,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };

            return response;
        }



        public async Task<ApiResponse<IEnumerable<AllLeadMasterListDTO>>> AllGetLeads(LeadFilterDto filter, int currentUserId)
        {
            var parameters = new[]
            {
        new SqlParameter("@userId", currentUserId),
        new SqlParameter("@mode", "All"),
        new SqlParameter("@stage", ""),
        new SqlParameter("@startDate", filter.StartDate ?? (object)DBNull.Value),
        new SqlParameter("@endDate", filter.EndDate ?? (object)DBNull.Value),
        new SqlParameter("@Source", filter.Source ?? ""),
        new SqlParameter("@Course", filter.Course ?? ""),
        new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 },
        new SqlParameter("@pageSize", filter.PageSize),
        new SqlParameter("@pageNumber", filter.PageNumber)
    };

            var finalList = await context.allLeadMasterListDTO
                .FromSqlRaw("EXEC dbo.GetLeadsList @userId, @mode, @stage, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
                .ToListAsync();

            var mappedResult = finalList.Select(l => new AllLeadMasterListDTO
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

            var totalRecords = finalList.Count; // You should ideally fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + finalList.Count - 1;

            var response = new ApiResponse<IEnumerable<AllLeadMasterListDTO>>
            {
                Response = mappedResult,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };

            return response;
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
        new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 },
        new SqlParameter("@pageSize", filter.PageSize),
        new SqlParameter("@pageNumber", filter.PageNumber)
    };

            var leads = await context.leadMasterAssignedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
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

            var totalRecords = leads.Count; // Ideally, fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + leads.Count - 1;

            var response = new ApiResponse<IEnumerable<LeadMasterAssignedListDTO>>
            {
                Response = result,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
                unReadCount = 0 // Replace with actual unread logic if applicable
            };

            return response;
        }




        public async  Task<ApiResponse<IEnumerable<LeadMasterContactedListDTO>>> GetContactedLeads(LeadFilterDto filter, int currentUserId)
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
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }, // Fetch all leads
    new SqlParameter("@pageSize", filter.PageSize),
    new SqlParameter("@pageNumber", filter.PageNumber)
      };


            var leads = await context.leadMasterContactedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
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
            var totalRecords = leads.Count; // You should ideally fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + leads.Count - 1;

            var response = new ApiResponse<IEnumerable<LeadMasterContactedListDTO>>
            {
                Response = result,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
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
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }, // Fetch all leads
    new SqlParameter("@pageSize", filter.PageSize),
    new SqlParameter("@pageNumber", filter.PageNumber)
};

            var leads = await context.leadMasterQualifiedListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
                .ToListAsync();

            var totalRecords = leads.Count; // You should ideally fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + leads.Count - 1;

            var response = new ApiResponse<IEnumerable<LeadMasterQualifiedListDTO>>
            {
                Response = leads,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
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
    new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }, // Fetch all leads
    new SqlParameter("@pageSize", filter.PageSize),
    new SqlParameter("@pageNumber", filter.PageNumber)
};

            var leads = await context.leadMasterFollowupListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
                .ToListAsync();


            var totalRecords = leads.Count; // You should ideally fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + leads.Count - 1;

            var response = new ApiResponse<IEnumerable<LeadMasterFollowupListDTO>>
            {
                Response = leads,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
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
   new SqlParameter("@assignedTo", SqlDbType.Int) { Value = 0 }, // Fetch all leads
    new SqlParameter("@pageSize", filter.PageSize),
    new SqlParameter("@pageNumber", filter.PageNumber)
};

            var leads = await context.leadMasterCounsellingListDTO
                .FromSqlRaw("EXEC GetLeadsList @userId, @mode, @status, @startDate, @endDate, @Source, @Course, @assignedTo, @pageSize, @pageNumber", parameters)
                .ToListAsync();
            var totalRecords = leads.Count; // You should ideally fetch total count from DB for real pagination
            var fromRecord = (filter.PageNumber - 1) * filter.PageSize + 1;
            var toRecord = fromRecord + leads.Count - 1;


            var response = new ApiResponse<IEnumerable<LeadMasterCounsellingListDTO>>
            {
                Response = leads,
                Succeded = true,
                totalRecords = totalRecords,
                fromRecord = fromRecord,
                torecord = toRecord,
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
                .FirstOrDefaultAsync(l => l.LeadId == leadId && l.IsDeleted== false);
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
