using Microsoft.EntityFrameworkCore;
using LMS.Components.ModelClasses.Leads;


namespace LMS.Components.Entities
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<UserEntity>().ToTable("Users");
			builder.Entity<UserTypeMasterEntity>().ToTable("UserTypeMaster");
			builder.Entity<ModuleMasterEntity>().ToTable("ModuleMaster");
			builder.Entity<ModuleRoleMapping>().ToTable("ModuleRoleMapping");
			builder.Entity<CourseTypesEntity>().ToTable("CourseTypes");
			builder.Entity<StatusTypes>().ToTable("StatusTypes");
			builder.Entity<CommonStatus>().ToTable("CommonStatus");
			builder.Entity<LeadMaster>().ToTable("LeadMaster");
			builder.Entity<LeadAudit>().ToTable("LeadAudit");
			builder.Entity<LeadNoteEntity>().ToTable("LeadNotes");
			builder.Entity<InstructorEntity>().ToTable("Instructors");
			builder.Entity<StudentEntity>().ToTable("Students");
			builder.Entity<FeeCollection>().ToTable("FeeCollection");
			builder.Entity<FeeReceipt>().ToTable("FeeReceipt");
			builder.Entity<CourseBatchEntity>().ToTable("CourseBatch");
			builder.Entity<ClassScheduleEntity>().ToTable("ClassSchedule");
            //modelBuilder.Entity<CountryModel>().HasNoKey();
            builder.Entity<LeadMasterUnAssignedListDTO>().HasNoKey();
            builder.Entity<LeadMasterAssignedListDTO>().HasNoKey();
            builder.Entity<LeadMasterContactedListDTO>().HasNoKey();
            builder.Entity<LeadMasterQualifiedListDTO>().HasNoKey();
            builder.Entity<LeadMasterFollowupListDTO>().HasNoKey();
            builder.Entity<LeadMasterCounsellingListDTO>().HasNoKey();
            builder.Entity<AllLeadMasterListDTO>().HasNoKey();
        }
        public DbSet<ClassScheduleEntity> ClassSchedule { get; set; }
        public DbSet<CourseBatchEntity> CourseBatches { get; set; }
        public DbSet<FeeCollection> FeeCollections { get; set; }
        public DbSet<FeeReceipt> FeeReceipts { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<InstructorEntity> instructorEntity { get; set; }
        public DbSet<AllLeadMasterListDTO> allLeadMasterListDTO { get; set; }
        public DbSet<LeadNoteEntity> leadNoteEntity { get; set; }
        public DbSet<LeadMasterCounsellingListDTO> leadMasterCounsellingListDTO { get; set; }
        public DbSet<LeadMasterFollowupListDTO> leadMasterFollowupListDTO { get; set; }
        public DbSet<LeadMasterQualifiedListDTO> leadMasterQualifiedListDTO { get; set; }
        public DbSet<LeadMasterContactedListDTO> leadMasterContactedListDTO { get; set; }
        public DbSet<LeadMasterAssignedListDTO> leadMasterAssignedListDTO { get; set; }
        public DbSet<LeadMasterUnAssignedListDTO> leadMasterUnAssignedListDTO { get; set; }
        public DbSet<UserEntity> userEntities { get; set; }
		public DbSet<UserTypeMasterEntity> userTypes { get; set; }
		public DbSet<ModuleMasterEntity> modules { get; set; }
		public DbSet<ModuleRoleMapping> mappings { get; set; }
		public DbSet<CourseTypesEntity> courseTypes { get; set; }
		public DbSet<StatusTypes> statusTypes { get; set; }
		public DbSet<CommonStatus> commonStatuses { get; set; }
		public DbSet<LeadMaster> leads { get; set; }
		public DbSet<LeadAudit> leadAudits { get; set; }
	}
}
