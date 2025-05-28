using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.Entities
{
    public class ModuleRoleMapping
    {
        [Key]
        public int MappingId { get; set; }
        public int? ModuleId { get; set; }
        public int? UserTypeId { get; set; }
        public bool? IsAccesible { get; set; }
        public bool? IsAddAccesible { get; set; }
        public bool? IsEditAccesible { get; set; }
        public bool? IsDeleteAccesible { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
