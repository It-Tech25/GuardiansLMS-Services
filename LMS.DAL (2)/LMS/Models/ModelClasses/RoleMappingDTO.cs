using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Components.ModelClasses
{
    public class RoleMappingDTO
    {
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public List<ModuleRoleData> modules { get; set; }
    }
    public class ModuleRoleData
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool IsAccesible { get; set; }
        public int UserTypeId { get; set; }     
        public string UserTypeName { get; set; }
        public List<SubModuleRoleData> subModules { get; set; }
        
    }
    public class SubModuleRoleData
    {
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public bool IsAccesible { get; set; }
        public bool? IsAddAccesible { get; set; }
        public bool? IsEditAccesible { get; set; }
        public bool? IsDeleteAccesible { get; set; }
    }


  

}
