using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.DepartmentDAL;
using thpt.ThachBan.DAL.DepartmentManagerDAL;

namespace thpt.ThachBan.BAL.DepartmentManagerBAL
{
    public class DepartmentManagerBAL:IDepartmentManagerBAL
    {
        private IDepartmentManagerDAL _departmentManagerDAL;
        public DepartmentManagerBAL(IDepartmentManagerDAL departmentManagerDAL)
        {
            _departmentManagerDAL= departmentManagerDAL;
        }
        public int AddDepartmentMNGById(Guid employeeId, List<Guid>? departId)
        {
            return _departmentManagerDAL.AddDepartmentMNGById(employeeId, departId);
        }
    }
}
