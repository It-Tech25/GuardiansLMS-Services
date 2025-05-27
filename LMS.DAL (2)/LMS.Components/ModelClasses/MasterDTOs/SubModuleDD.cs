using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses.MasterDTOs
{
    public class SubModuleDD
    {
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
    }

    public class SubModuleListDTO
    {
        public int SubModuleId { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
    }
    
    public class SubModuleDTO
    {
        public int SubModuleId { get; set; }
        public int ModuleId { get; set; }
        public string SubModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public string Description { get; set; }
    }
}
