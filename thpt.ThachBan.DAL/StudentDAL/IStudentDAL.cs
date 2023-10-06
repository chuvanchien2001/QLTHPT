using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.ViewModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Common;
using thpt.ThachBan.DTO.ViewModels.Areas.Student;

namespace thpt.ThachBan.DAL.StudentDAL
{
    public interface IStudentDAL
    {
        /// <summary>
        /// lấy các thông tin trả về cho view AboutStudent
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Thông tin cho view AboutStudent</returns>
        public AboutStudent GetAboutStudent(string code);
        /// <summary>
        /// lấy các thông tin trả về cho view AboutStudent
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Thông tin cho view AboutStudent</returns>
        public AboutStudent GetAboutStudent(Guid id);

        /// <summary>
        /// Phân trang học sinh
        /// </summary>
        /// <param name="CodeSearch"></param>
        /// <param name="NameSearch"></param>
        /// <param name="ClassSearch"></param>
        /// <param name="TaskSearch"></param>
        /// <param name="PolicyName"></param>
        /// <returns></returns>
        //public List<AboutStudent> StudentPaging(string CodeSearch = null, string NameSearch = null, string ClassSearch = null, string TaskSearch = null, string PolicyName = null);
    }
}
