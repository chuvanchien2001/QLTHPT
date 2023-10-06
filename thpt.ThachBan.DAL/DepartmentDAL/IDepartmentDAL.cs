using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.DepartmentDAL
{
    public interface IDepartmentDAL
    {
        public List<Department> GetDepartmentOfEmployee(Guid employeeId);
        public List<Department> GetDepartmentByNameList(List<string> departmentNames);
    }
}
