using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thpt.ThachBan.BAL.DepartmentManagerBAL
{
    public interface IDepartmentManagerBAL
    {
        public int AddDepartmentMNGById(Guid employeeId, List<Guid>? departId);
    }
}
