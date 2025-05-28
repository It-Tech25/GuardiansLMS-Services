using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;
using LMS.DAL.Interfaces;

namespace LMS.DAL.Repositories
{
    public class CommonDDRepo : ICommonDDRepo
    {
        private readonly MyDbContext context;
        public CommonDDRepo(MyDbContext _context)
        {
            context = _context;
        }

        public List<StatusDD> GetStatusDetails(string TypeName)
        {
            List<StatusDD> res = new List<StatusDD>();
            try
            {
                res = (from s in context.commonStatuses
                       join t in context.statusTypes on s.StatusTypeId equals t.TypeId
                       where s.IsDeleted==false &&t.TypeName==TypeName
                       select new StatusDD
                       {
                           Id=s.StatusId,
                           Status = s.StatusName
                       }).ToList();
            }catch(Exception e) { }
            return res;
        }

        public StatusTypes GetStatusTypeByName(string TypeName)
        {
            StatusTypes res = new StatusTypes();
            try
            {
                res = context.statusTypes.Where(t => t.TypeName == TypeName && t.IsDeleted == false).FirstOrDefault();
            }catch( Exception e) { }
            return res;
        }
        public CommonStatus GetStatusByName(string Name,int TypeId)
        {
            CommonStatus res = new CommonStatus();
            try
            {
                res = context.commonStatuses.Where(s => s.StatusName == Name && s.IsDeleted == false && s.StatusTypeId == TypeId).FirstOrDefault();
            }catch (Exception e) { }
            return res;
        }
    }
}
