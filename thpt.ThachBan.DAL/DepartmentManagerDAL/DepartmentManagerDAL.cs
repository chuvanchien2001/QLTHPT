using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.DepartmentManagerDAL
{
    public class DepartmentManagerDAL:IDepartmentManagerDAL
    {
        public int AddDepartmentMNGById(Guid employeeId, List<Guid>? departId)
        {
            int rowEffeact = 0;
            List<DepartmentManager> listDepart = DatabaseContext.GetDB.DepartmentManager.Where(x => x.EmployeeId == employeeId).ToList();
            DatabaseContext.GetDB.RemoveRange(listDepart);
            foreach (Guid guidid in departId)
            {
                DepartmentManager MNGDepart = new DepartmentManager();
                MNGDepart.DepartmentManagerId = new Guid();
                MNGDepart.EmployeeId = employeeId;
                MNGDepart.DepartmentId = guidid;
                DatabaseContext.GetDB.DepartmentManager.Add(MNGDepart);
                rowEffeact++;

            }
            DatabaseContext.GetDB.SaveChanges();
            return rowEffeact;
        }
    }
}
