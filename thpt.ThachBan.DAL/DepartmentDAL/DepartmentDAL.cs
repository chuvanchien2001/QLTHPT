using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.DepartmentDAL
{
    public class DepartmentDAL: IDepartmentDAL
    {
        public List<Department> GetDepartmentOfEmployee(Guid employeeId)
        {
            return (from departMng in DatabaseContext.GetDB.DepartmentManager.Where(x=>x.EmployeeId== employeeId)
                   join d in DatabaseContext.GetDB.Department
                   on departMng.DepartmentId equals d.DepartmentId
                   select d).ToList();  
        }
        public List<Department> GetDepartmentByNameList(List<string> departmentNames)
        {
            List<Department> departments = new List<Department>();
            foreach (string departmentName in departmentNames)
            {
                departments.Add(DatabaseContext.GetDB.Department.Where(x => x.DepartmentName == departmentName).FirstOrDefault());
            }
            return departments;
        }

    }
}
