using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.Entities
{
    public class ModuleMasterEntity
    {
        [Key]
        public int ModuleId { get; set; }
        public string? ModuleName { get; set; }
        public string? ModuleIcon { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public int? DeactivatedBy { get; set; }
        public DateTime? DeactivatedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
