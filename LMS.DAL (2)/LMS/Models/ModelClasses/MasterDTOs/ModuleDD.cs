using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.MasterDTOs
{
    public class ModuleDD
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }
    public class ModuleDTO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string? ModuleIcon { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
    }
    public class ModuleAddDTO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string? ModuleIcon { get; set; }
        public string Description { get; set; }
    }
}
