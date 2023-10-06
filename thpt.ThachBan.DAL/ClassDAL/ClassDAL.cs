using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.ClassDAL
{
    public class ClassDAL:IClassDAL
    {
        public List<Class> GetClassByNameList(List<string> Names)
        {
            List<Class> classes = new List<Class>();
            foreach (string className in Names)
            {
                classes.Add(DatabaseContext.GetDB.Class.Where(x => x.ClassName == className).FirstOrDefault());
            }
            return classes;
        }
    }
}
