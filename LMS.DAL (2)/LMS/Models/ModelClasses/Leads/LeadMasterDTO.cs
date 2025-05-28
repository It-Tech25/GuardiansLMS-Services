using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Components.ModelClasses.Leads
{
    public class LeadMasterListDTO
    {
        public int LeadId { get; set; }
        public string FromSource { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? IntrestedCourse { get; set; }
        public string? AssignedUser { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }

    public class AddLeadModel
    {
        public int LeadId { get; set; }
        public string FromSource { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? IntrestedCourse { get; set; }
        public int? AssignedUser { get; set; }
    }
    
    public class EditLeadModel
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? IntrestedCourse { get; set; }
        public int? AssignedUser { get; set; }
        public int? StatusId { get; set; }
    }

    public class LeadMasterUnAssignedListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }
        
        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }
    public class LeadMasterAssignedListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }

        [Column("UserName")]
        public string? AssignedUser { get; set; }
        //public DateTime? AssignedOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }
    public class LeadMasterContactedListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }

        [Column("UserName")]
        public string? AssignedUser { get; set; }

        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        //public DateTime? ContactedOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }
    public class LeadMasterQualifiedListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }

        [Column("UserName")]
        public string? AssignedUser { get; set; }

        public bool IsShowReAsign { get; set; }
    }

    //public class LeadMasterQualifiedListDTO
    //{
    //    public int LeadId { get; set; }
    //    public string FromSource { get; set; }
    //    public string MobileNumber { get; set; }
    //    public string Email { get; set; }
    //    public string Name { get; set; }
    //    public string? IntrestedCourse { get; set; }
    //    public string? AssignedUser { get; set; }
    //    public DateTime? QualifiedOn { get; set; }  // ✅ Added Qualified Date
    //    public string? Status { get; set; }  // ✅ Status field
    //    public string? Budget { get; set; }  // ✅ Budget field
    //    public string? Timeline { get; set; }  // ✅ Timeline field
    //    public string? Needs { get; set; }  // ✅ Needs field
    //    public bool IsShowReAsign { get; set; }
    //}

    public class LeadMasterFollowupListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }

        [Column("UserName")]
        public string? AssignedUser { get; set; }
        public DateTime? LastFollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public bool IsShowReAsign { get; set; }
    }
    public class LeadMasterCounsellingListDTO
    {
      public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }
        [Column("UserName")]
        public string? AssignedUser { get; set; }
      //  public DateTime? DemoOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }


    public class LeadFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Source { get; set; } = "";
        public string Course { get; set; } = "";
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;

        
        //public string? Status { get; set; }  
        //public int? AssignedTo { get; set; } 
    }



    public class UpdateLeadStatusDto
    {
      public int LeadId { get; set; }
      public int userid { get; set; } 
   
    }

    public class AddLeadNoteDto
    {
        public int LeadId { get; set; }
        public string? NoteText { get; set; }
        
    }



    public class AllLeadMasterListDTO
    {
        public int LeadId { get; set; }
        public string? FromSource { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? InterestedCourse { get; set; }

        [Column("UserName")]
        public string? AssignedUser { get; set; }

        [Column("StatusName")]
        public string? Status { get; set; }

        [Column("CreatedOn")]
        public DateTime? CreatedOn { get; set; }
        public bool IsShowReAsign { get; set; }
    }

    public class LeadStatusUpdateRequest
    {
        public int LeadId { get; set; }
        public string NextStatus { get; set; }
        public int AssignedUserId { get; set; }
        public int ModifiedBy { get; set; }
    }



}


