using LMS.BAL.Interfaces;
using LMS.Components.Entities;
using LMS.Components.ModelClasses;
using LMS.Components.ModelClasses.Common;
using LMS.Components.ModelClasses.MasterDTOs;
using LMS.DAL.Interfaces;

namespace LMS.BAL.Services
{
    public class ModuleMgmtService : IModuleMgmtService
    {
        private readonly IModuleMgmtRepo mRepo;
        private readonly IMasterMgmtRepo masterRepo;

        public ModuleMgmtService(IModuleMgmtRepo mRepo, IMasterMgmtRepo _masterRepo)
        {
            this.mRepo = mRepo;
            this.masterRepo = _masterRepo;
        }

        #region Module and Sub Module

        public List<ModuleDTO> GetModulesList(string search = "")
        {
            return mRepo.GetModulesList(search);
        }

        public List<ModuleDD> GetModulesDD()
        {
            return mRepo.GetModulesDD();
        }

        public ModuleDTO GetModuleById(int id)
        {
            return mRepo.GetModuleById(id);
        }

        public GenericResponse AddEditModule(ModuleAddDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            ModuleMasterEntity m = new ModuleMasterEntity();
            ModuleMasterEntity om = mRepo.GetModuleDataByName(req.ModuleName);

            if (req.ModuleId == 0)
            {
                if (om == null)
                {
                    m.ModuleName = req.ModuleName;
                    m.ParentId = null;
                    m.Description = req.Description;
                    m.ModuleIcon = req.ModuleIcon;
                    m.CreatedBy = currentUserId;
                    m.CreatedOn = DateTime.Now;
                    m.IsActive = true;
                    m.IsDeleted = false;
                    res = mRepo.AddModule(m);
                }
                else
                {
                    res.statusCode = 0;
                    res.Message = "Name is already used";
                }
            }
            else
            {
                bool isDuplicate = false;
                if (om != null)
                {
                    if (req.ModuleId != om.ModuleId)
                    {
                        isDuplicate = true;
                    }
                }
                if (isDuplicate)
                {
                    res.statusCode = 0;
                    res.Message = "Name is already used";
                }
                else
                {
                    m = mRepo.GetModuleDataById(req.ModuleId);
                    if (m.ModuleId == 0)
                    {
                        res.statusCode = 0;
                        res.Message = "Id does not exists";
                    }
                    else
                    {
                        m.ModuleName = req.ModuleName;
                        m.Description = req.Description;
                        m.ModuleIcon = req.ModuleIcon;
                        m.ModifiedBy = currentUserId;
                        m.ModifiedOn = DateTime.Now;
                        res = mRepo.UpdateModule(m);
                    }
                }
            }
            return res;
        }

        public GenericResponse AddEditSubModule(SubModuleDTO req, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            ModuleMasterEntity m = new ModuleMasterEntity();
            ModuleMasterEntity om = mRepo.GetSubModuleDataByName(req.SubModuleName, req.ModuleId);

            if (req.SubModuleId == 0)
            {
                if (om != null)
                {
                    m.ModuleName = req.SubModuleName;
                    m.ParentId = req.ModuleId;
                    m.Description = req.Description;
                    m.ModuleIcon = req.ModuleIcon;
                    m.CreatedBy = currentUserId;
                    m.CreatedOn = DateTime.Now;
                    m.IsActive = true;
                    m.IsDeleted = false;
                    res = mRepo.AddModule(m);
                }
                else
                {
                    res.statusCode = 0;
                    res.Message = "Name is already used";
                }
            }
            else
            {
                if (om == null)
                {
                    res.statusCode = 0;
                    res.Message = "Name is already used";
                }
                else
                {
                    if (om.ModuleId == req.ModuleId)
                    {
                        m = mRepo.GetModuleDataById(req.ModuleId);
                        if (m.ModuleId == 0)
                        {
                            res.statusCode = 0;
                            res.Message = "Id does not exists";
                        }
                        else
                        {
                            m.ModuleName = req.SubModuleName;
                            m.Description = req.Description;
                            m.ModuleIcon = req.ModuleIcon;
                            m.ModifiedBy = currentUserId;
                            m.ModifiedOn = DateTime.Now;
                            res = mRepo.UpdateModule(m);
                        }
                    }
                    else
                    {
                        res.statusCode = 0;
                        res.Message = "Name is already used";
                    }
                }
            }
            return res;
        }

        public GenericResponse ActivateModule(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            ModuleMasterEntity m = mRepo.GetModuleDataById(id);
            if (m.ModuleId != 0)
            {
                if (m.IsActive == true)
                {
                    res.statusCode = 0;
                    res.Message = "The module is already Active";
                }
                else
                {
                    m.IsActive = true;
                    res = mRepo.UpdateModule(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public GenericResponse DeActivateModule(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            ModuleMasterEntity m = mRepo.GetModuleDataById(id);
            if (m.ModuleId != 0)
            {
                if (m.IsActive == false)
                {
                    res.statusCode = 0;
                    res.Message = "The module is already De activated";
                }
                else
                {
                    m.IsActive = false;
                    m.DeactivatedBy = currentUserId;
                    m.DeactivatedOn = DateTime.Now;
                    res = mRepo.UpdateModule(m);
                }
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public GenericResponse DeleteModule(int id, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            ModuleMasterEntity m = mRepo.GetModuleDataById(id);
            if (m.ModuleId != 0)
            {
                m.IsDeleted = true;
                m.ModifiedBy = currentUserId;
                m.ModifiedOn = DateTime.Now;
                res = mRepo.UpdateModule(m);
            }
            else
            {
                res.statusCode = 0;
                res.Message = "Id does not exists";
            }
            return res;
        }

        public List<SubModuleListDTO> GetSubModulesList(int moduleId, string search = "")
        {
            return mRepo.GetSubModulesList(moduleId, search);
        }

        public List<SubModuleDD> GetSubModulesDD(int moduleId)
        {
            return mRepo.GetSubModulesDD(moduleId);
        }

        public SubModuleDTO GetSubModuleById(int id)
        {
            return mRepo.GetSubModuleById(id);
        }

        #endregion

        #region RoleMappping

        public RoleMappingDTO GetUserRoleMappingData(int userTypeId)
        {
            RoleMappingDTO res = new RoleMappingDTO();
            try
            {
                UserTypeDTO ut = masterRepo.GetUserTypeById(userTypeId);
                res.UserTypeId = userTypeId;
                res.UserTypeName = ut.UserTypeName;
                res.modules = new List<ModuleRoleData>();
                List<ModuleDTO> modules = mRepo.GetModulesList();
                foreach (ModuleDTO mm in modules)
                {
                    ModuleRoleData m = new ModuleRoleData();
                    m.ModuleId = mm.ModuleId;
                    m.ModuleName = mm.ModuleName;
                    m.UserTypeId = userTypeId;
                    m.UserTypeName = ut.UserTypeName;
                    m.subModules = new List<SubModuleRoleData>();
                    List<SubModuleListDTO> submodules = mRepo.GetSubModulesList(mm.ModuleId);

                    foreach (SubModuleListDTO smm in submodules)
                    {
                        SubModuleRoleData sm = new SubModuleRoleData();
                        sm.SubModuleId = smm.SubModuleId;
                        sm.SubModuleName = smm.ModuleName;
                        ModuleRoleMapping r = mRepo.GetRoleMapppingByRoleAndModuleId(userTypeId, smm.SubModuleId);
                        if (r != null)
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
                            sm.IsAddAccesible = r.IsAddAccesible;
                            sm.IsEditAccesible = r.IsEditAccesible;
                            sm.IsDeleteAccesible = r.IsDeleteAccesible;
                        }
                        else
                        {
                            sm.IsAccesible = false;
                        }
                        m.subModules.Add(sm);
                    }
                    res.modules.Add(m);
                }
            }
            catch (Exception ex) { }
            return res;
        }

        public GenericResponse UpdateRoleMappping(RoleMappingDTO data, int currentUserId)
        {
            GenericResponse res = new GenericResponse();
            try
            {
                foreach (ModuleRoleData m in data.modules)
                {
                    foreach (SubModuleRoleData sm in m.subModules)
                    {
                        ModuleRoleMapping r = mRepo.GetRoleMapppingByRoleAndModuleId(data.UserTypeId, sm.SubModuleId);
                        if (r != null)
                        {
                            r.IsAccesible = sm.IsAccesible;
                            r.IsAddAccesible = sm.IsAddAccesible;
                            r.IsEditAccesible = sm.IsEditAccesible;
                            r.IsDeleteAccesible = sm.IsDeleteAccesible;
                            r.ModifiedOn = DateTime.Now;
                            r.ModifiedBy = currentUserId;
                            res = mRepo.UpdateRoleMappping(r);
                        }
                        else
                        {
                            ModuleRoleMapping rm = new ModuleRoleMapping();
                            rm.IsDeleted = false;
                            rm.IsAddAccesible = sm.IsAddAccesible;
                            rm.IsEditAccesible = sm.IsEditAccesible;
                            rm.IsDeleteAccesible = sm.IsDeleteAccesible;
                            rm.UserTypeId = data.UserTypeId;
                            rm.ModuleId = sm.SubModuleId;
                            rm.IsAccesible = sm.IsAccesible;
                            rm.CreatedOn = DateTime.Now;
                            rm.CreatedBy = currentUserId;
                            res = mRepo.AddRoleMappping(rm);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return res;
        }

        #endregion
    }
}
