using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
    public class LeadNoteEntity
    {
        [Key]
        public int NoteId { get; set; }
        public int LeadId { get; set; }
        public string NoteText { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
