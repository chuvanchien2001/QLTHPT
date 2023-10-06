using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.ClassDAL
{
    public interface IClassDAL
    {
        public List<Class> GetClassByNameList(List<string> Names);
    }
}
