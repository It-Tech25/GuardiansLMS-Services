using LMS.Components.Entities;
using LMS.Components.ModelClasses.Common;

namespace LMS.DAL.Interfaces
{
    public interface ICommonDDRepo
    {
        List<StatusDD> GetStatusDetails(string TypeName);
        StatusTypes GetStatusTypeByName(string TypeName);
        CommonStatus GetStatusByName(string Name, int TypeId);
    }
}
