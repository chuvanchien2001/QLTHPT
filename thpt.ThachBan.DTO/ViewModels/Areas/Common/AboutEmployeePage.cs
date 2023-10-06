using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Common
{
    public class AboutEmployeePage
    {
        public Employee Employee=new Employee();
        public List<Class> Classes=new List<Class>();
        public List<Department> Departments=new List<Department>();
        public List<Subject> Subjects=new List<Subject>();
    }
}
