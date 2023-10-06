using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DAL.DepartmentDAL;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.DepartmentBAL
{
    public class DepartmentBAL:IDepartmentBAL
    {
        private IDepartmentDAL _IDepartmentDAL;
        public DepartmentBAL(IDepartmentDAL IDepartmentDAL)
        {
            _IDepartmentDAL=IDepartmentDAL;
        }
        public List<Department> GetDepartmentOfEmployee(Guid employeeId)
        {
            return _IDepartmentDAL.GetDepartmentOfEmployee(employeeId);
        }
        public List<Department> GetDepartmentByNameList(List<string> departmentNames)
        {
            return _IDepartmentDAL.GetDepartmentByNameList(departmentNames);
        }
    }
}
