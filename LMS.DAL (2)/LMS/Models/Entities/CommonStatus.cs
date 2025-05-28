using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
    public class CommonStatus
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? StatusTypeId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
