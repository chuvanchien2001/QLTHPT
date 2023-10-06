using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.ClassBAL
{
    public interface IClassBAL
    {
        public List<Class> GetClassByNameList(List<string> Names);
    }
}
