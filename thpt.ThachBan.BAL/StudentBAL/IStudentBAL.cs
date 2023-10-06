using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.ViewModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Student;

namespace thpt.ThachBan.BAL.StudentBAL
{
    public interface IStudentBAL
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
    }
}
