using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thpt.ThachBan.DAL.DepartmentManagerDAL
{
    public interface IDepartmentManagerDAL
    {
        public int AddDepartmentMNGById(Guid employeeId, List<Guid>? departId);
    }
}
