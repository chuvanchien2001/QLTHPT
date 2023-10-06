using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.ClassDAL;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.ClassBAL
{
    public class ClassBAL:IClassBAL
    {
        private IClassDAL classDAL;
        public ClassBAL(IClassDAL classDAL)
        {
            this.classDAL = classDAL;
        }
        public List<Class> GetClassByNameList(List<string> Names)
        {
            return classDAL.GetClassByNameList(Names);
        }
    }
}
