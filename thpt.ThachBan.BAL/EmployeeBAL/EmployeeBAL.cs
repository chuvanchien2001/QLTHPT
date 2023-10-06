using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.EmployeeDAL;
using thpt.ThachBan.DTO.ViewModels.Areas.Common;

namespace thpt.ThachBan.BAL.EmployeeBAL
{
    public class EmployeeBAL:IEmployeeBAL
    {
        private IEmployeeDAL employeeDAL;
        public EmployeeBAL(IEmployeeDAL employeeDAL)
        {
            this.employeeDAL = employeeDAL;
        }

        /// <summary>
        /// lây thông tin của employee
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public AboutEmployeePage GetAboutEmployee(string code)
        {
            return employeeDAL.GetAboutEmployee(code);
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
        public List<AboutEmployeePage> EmployeePaging(int OrderBy,int Status,string CodeSearch = null, string NameSearch = null, string ClassSearch = null, string SubjectSearch = null, string NumberPhoneSearch = null)
        {
            return employeeDAL.EmployeePaging(OrderBy,CodeSearch, NameSearch, ClassSearch, SubjectSearch, NumberPhoneSearch).Where(x=>x.Employee.Status== Status).ToList();
        }
    }
}
