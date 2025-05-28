using LMS.Components.Entities;
using LMS.Components.ModelClasses;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DAL.Repositories
{
    public class ModuleMgmtRepo: IModuleMgmtRepo
    {
        private readonly MyDbContext context;
        public ModuleMgmtRepo(MyDbContext _context)
        {
            context = _context;
        }

        #region Module

        public List<ModuleDTO> GetModulesList(string search = "")
        {
            List<ModuleDTO> response = new List<ModuleDTO>();
            try
            {
                response = (from m in context.modules
                            where m.IsDeleted == false && m.ModuleName.Contains(search) && m.ParentId == null
                            select new ModuleDTO
                            {
                                ModuleId = m.ModuleId,
                                ModuleName = m.ModuleName,
                                Description = m.Description,
                                ModuleIcon = m.ModuleIcon,
                                CurrentStatus = m.IsActive == true ? "Active" : "In-Active"
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }
        
        public List<ModuleDD> GetModulesDD()
        {
            List<ModuleDD> response = new List<ModuleDD>();
            try
            {
                response = (from m in context.modules
                            where m.IsDeleted == false && m.ParentId == null && m.IsActive == true
                            select new ModuleDD
                            {
                                ModuleId = m.ModuleId,
                                ModuleName = m.ModuleName
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public ModuleDTO GetModuleById(int id)
        {
            ModuleDTO response = new ModuleDTO();
            try
            {
                response = (from m in context.modules
                            where m.IsDeleted == false && m.ModuleId == id && m.ParentId == null
                            select new ModuleDTO
                            {
                                ModuleId = m.ModuleId,
                                ModuleName = m.ModuleName,
                                Description = m.Description,
                                ModuleIcon = m.ModuleIcon,
                            }).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }
        
        public ModuleMasterEntity GetModuleDataById(int id)
        {
            ModuleMasterEntity response = new ModuleMasterEntity();
            try
            {
                response = context.modules.Where(m => m.ModuleId == id && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }
        
        public ModuleMasterEntity GetModuleDataByName(string name)
        {
            ModuleMasterEntity response = new ModuleMasterEntity();
            try
            {
                response = context.modules.Where(m => m.ModuleName == name.Trim() && m.IsDeleted == false && m.ParentId == null).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public GenericResponse AddModule(ModuleMasterEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.modules.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.ModuleId;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateModule(ModuleMasterEntity req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.modules.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.ModuleId;
            }
            catch (Exception ex)
            {
                response.statusCode = 404;
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion
        
        #region Sub Module

        public List<SubModuleListDTO> GetSubModulesList(int moduleId,string search = "")
        {
            List<SubModuleListDTO> response = new List<SubModuleListDTO>();
            try
            {
                if (moduleId > 0)
                {
                    response = (from m in context.modules
                                join p in context.modules on m.ParentId equals p.ModuleId
                                where m.IsDeleted == false && m.ModuleName.Contains(search) && m.ParentId == moduleId
                                select new SubModuleListDTO
                                {
                                    SubModuleId = m.ModuleId,
                                    ModuleName = m.ModuleName,
                                    Description = m.Description,
                                    ModuleIcon = m.ModuleIcon,
                                    SubModuleName = p.ModuleName,
                                    CurrentStatus = m.IsActive == true ? "Active" : "In-Active"
                                }).ToList();
                }
                else
                {
                    response = (from m in context.modules
                                join p in context.modules on m.ParentId equals p.ModuleId
                                where m.IsDeleted == false && m.ModuleName.Contains(search) && m.ParentId >0
                                select new SubModuleListDTO
                                {
                                    SubModuleId = m.ModuleId,
                                    SubModuleName = m.ModuleName,
                                    Description = m.Description,
                                    ModuleIcon = m.ModuleIcon,
                                    ModuleName = p.ModuleName,
                                    CurrentStatus = m.IsActive == true ? "Active" : "In-Active"
                                }).ToList();
                }
            }
            catch (Exception ex) { }
            return response;
        }
        
        public List<SubModuleDD> GetSubModulesDD(int moduleId)
        {
            List<SubModuleDD> response = new List<SubModuleDD>();
            try
            {
                response = (from m in context.modules
                            where m.IsDeleted == false && m.ParentId == moduleId && m.IsActive == true
                            select new SubModuleDD
                            {
                                SubModuleId = m.ModuleId,
                                SubModuleName = m.ModuleName
                            }).ToList();
            }
            catch (Exception ex) { }
            return response;
        }

        public SubModuleDTO GetSubModuleById(int id)
        {
            SubModuleDTO response = new SubModuleDTO();
            try
            {
                response = (from m in context.modules
                            join p in context.modules on m.ParentId equals p.ModuleId
                            where m.IsDeleted == false && m.ModuleId == id 
                            select new SubModuleDTO
                            {
                                SubModuleId = m.ModuleId,
                                SubModuleName = m.ModuleName,
                                Description = m.Description,
                                ModuleIcon = m.ModuleIcon,
                                ModuleId = p.ModuleId
                            }).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public ModuleMasterEntity GetSubModuleDataByName(string name,int moduleId)
        {
            ModuleMasterEntity response = new ModuleMasterEntity();
            try
            {
                response = context.modules.Where(m => m.ModuleName == name.Trim() && m.IsDeleted == false && m.ParentId == moduleId).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        #endregion

        #region RoleMappping

        public ModuleRoleMapping GetRoleMapppingDataById(int id)
        {
            ModuleRoleMapping response = new ModuleRoleMapping();
            try
            {
                response = context.mappings.Where(m => m.MappingId == id && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public ModuleRoleMapping GetRoleMapppingByRoleAndModuleId(int usertypeId,int moduleId)
        {
            ModuleRoleMapping response = new ModuleRoleMapping();
            try
            {
                response = context.mappings.Where(m => m.ModuleId == moduleId && m.UserTypeId == usertypeId && m.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex) { }
            return response;
        }

        public GenericResponse AddRoleMappping(ModuleRoleMapping req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.mappings.Add(req);
                context.SaveChanges();
                response.statusCode = 201;
                response.Message = "Created";
                response.CurrentId = req.ModuleId;
            }
            catch (Exception ex)
            {
                response.statusCode = 0;
                response.Message = ex.Message;
            }
            return response;
        }

        public GenericResponse UpdateRoleMappping(ModuleRoleMapping req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                context.mappings.Update(req);
                context.SaveChanges();
                response.statusCode = 200;
                response.Message = "Updated";
                response.CurrentId = req.ModuleId;
            }
            catch (Exception ex)
            {
                response.statusCode = 404;
                response.Message = ex.Message;
            }
            return response;
        }

        public RoleMappingDTO GetUserRoleMappingData(int userTypeId)
        {
            RoleMappingDTO res = new RoleMappingDTO();
            try
            {
                UserTypeMasterEntity ut = context.userTypes.Where(t => t.UserTypeId == userTypeId).FirstOrDefault();
                res.UserTypeId = userTypeId;
                res.UserTypeName = ut.UserTypeName;
                res.modules = new List<ModuleRoleData>();
                List<ModuleMasterEntity> modules = context.modules.Where(a => a.IsDeleted == false && a.ParentId == null).ToList();
                foreach(ModuleMasterEntity mm in modules)
                {
                    ModuleRoleData m = new ModuleRoleData();
                    m.ModuleId = mm.ModuleId;
                    m.ModuleName = mm.ModuleName;
                    m.subModules = new List<SubModuleRoleData>();
                    List<ModuleMasterEntity> submodules = context.modules.Where(a => a.IsDeleted == false && a.ParentId == mm.ModuleId).ToList();

                    foreach(ModuleMasterEntity smm in submodules)
                    {
                        SubModuleRoleData sm = new SubModuleRoleData();
                        sm.SubModuleId = smm.ModuleId;
                        sm.SubModuleName = smm.ModuleName;
                        ModuleRoleMapping r = context.mappings.Where(r => r.ModuleId == smm.ModuleId && r.UserTypeId == userTypeId && r.IsDeleted == false).FirstOrDefault();
                        if(r != null)
                        {
                            if (r.IsAccesible == true)
                            {
                                sm.IsAccesible = true;
                                m.IsAccesible = true;
                            }
                            else
                            {
                                sm.IsAccesible = false;
                            }
                        }
                        else
                        {
                            sm.IsAccesible = false;
                        }
                        m.subModules.Add(sm);
                    }
                    res.modules.Add(m);
                }
            }catch(Exception ex) { }
            return res;
        }

        #endregion
    }
}
