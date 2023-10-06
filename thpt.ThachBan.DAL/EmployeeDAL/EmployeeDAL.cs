using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels.Areas.Common;

namespace thpt.ThachBan.DAL.EmployeeDAL
{
    public class EmployeeDAL:IEmployeeDAL
    {
        /// <summary>
        /// lây thông tin của employee
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AboutEmployeePage GetAboutEmployee(string code)
        {
            AboutEmployeePage aboutEmployee = new AboutEmployeePage();
            // lấy thông tin học sinh
            var employees = DatabaseContext.GetDB.Employee.Include(e => e.Subject).Where(x => x.EmployeeCode == code).ToList();
            aboutEmployee.Employee = employees.FirstOrDefault();
            // lấy ds contact
            aboutEmployee.Departments = (from e in employees
                                         join deMNG in DatabaseContext.GetDB.DepartmentManager
                                         on e.EmployeeId equals deMNG.EmployeeId
                                         join d in DatabaseContext.GetDB.Department
                                         on deMNG.DepartmentId equals d.DepartmentId
                                         select d).ToList();
            aboutEmployee.Subjects = aboutEmployee.Employee.Subject.ToList();
            aboutEmployee.Classes = DatabaseContext.GetDB.Class.Where(x => x.EmployeeId == aboutEmployee.Employee.EmployeeId).ToList();
            return aboutEmployee;
        }

        /// <summary>
        /// Phân trang nhân viên
        /// </summary>
        /// <param name="CodeSearch">Mã cần tìm</param>
        /// <param name="NameSearch">Tên cần tìm</param>
        /// <param name="ClassSearch">Lớp cần tìm</param>
        /// <param name="SubjectSearch">Môn học cần tìm</param>
        /// <param name="NumberPhoneSearch">Số điện thoại cần tìm</param>
        /// <returns>danh sách sau khi lọc qua các điều kiện</returns>
        public List<AboutEmployeePage> EmployeePaging(int OrderBy, string CodeSearch = null, string NameSearch = null, string ClassSearch = null, string SubjectSearch = null, string NumberPhoneSearch = null)
        {
            List<AboutEmployeePage> aboutEmployees = new List<AboutEmployeePage>();
            foreach (var item in DatabaseContext.GetDB.Employee.ToList())
            {
                AboutEmployeePage aboutEmployee = GetAboutEmployee(item.EmployeeCode);
                aboutEmployees.Add(aboutEmployee);
            }
            aboutEmployees = aboutEmployees.ToList();
            if (OrderBy == 0)
            {
                aboutEmployees = aboutEmployees.OrderBy(x => x.Employee.EmployeeCode).ToList();
            }
            else if (OrderBy == 1)
            {
                aboutEmployees = aboutEmployees.OrderBy(x => x.Employee.CreatedDate).Reverse().ToList();
            }
            else if (OrderBy == 2)
            {
                aboutEmployees = aboutEmployees.OrderBy(x => x.Employee.EmployeeName).ToList();
            }
            if (!String.IsNullOrEmpty(CodeSearch))
            {
                aboutEmployees = aboutEmployees.Where(p => p.Employee.EmployeeCode.Contains(CodeSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(NameSearch))
            {
                aboutEmployees = aboutEmployees.Where(p => p.Employee.EmployeeName.ToLower().Contains(NameSearch.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(ClassSearch))
            {
                aboutEmployees = aboutEmployees.Where(p => String.Join(", ", p.Classes.Select(x => x.ClassName)).ToLower()
                .Contains(ClassSearch.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(SubjectSearch))
            {
                aboutEmployees = aboutEmployees.Where(p => String.Join(", ", p.Subjects.Select(x => x.SubjectName)).ToLower().Contains(SubjectSearch.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(NumberPhoneSearch))
            {
                aboutEmployees = aboutEmployees.Where(p => p.Employee.PhoneNumber.Contains(NumberPhoneSearch)).ToList();
            }
            return aboutEmployees;
        }
    }
}
