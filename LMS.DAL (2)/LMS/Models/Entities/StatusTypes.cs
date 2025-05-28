using System.ComponentModel.DataAnnotations;

namespace LMS.Components.Entities
{
    public class StatusTypes
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
