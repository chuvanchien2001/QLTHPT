using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.AccountDAL
{
    public class AccountDAL:IAccountDAL
    {
        /// <summary>
        /// Kiểm tra thông tin đăng nhập và trạng thái hoạt động của tài khoản
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>trả về tk nếu khớp, ngược lại thì null</returns>
        public Account? CheckLoginInfor(string username, string password)
        {
            Account? acc= DatabaseContext.GetDB.Account.Where(x =>
            x.AccountCode == username && x.Password == password)
                .FirstOrDefault();
            if (acc != null)
            {
                acc.Role = DatabaseContext.GetDB.Role.Find(acc.RoleId);
                acc.Role.License = DatabaseContext.GetDB.License.Where(x => x.RoleId == acc.RoleId).ToList();
            }
            return acc;
        }

        /// <summary>
        /// Kiểm tra thông tin xác nhận
        /// </summary>
        /// <param name="type">Loại tài khoản</param>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="email">email</param>
        /// <param name="phonenumber">số điện thoại</param>
        /// <returns>true nếu đúng, false nếu toàn bộ thông tin sai</returns>
     
        public bool CheckInformationForgot(string type, string username, string email, string phonenumber)
        {
            if (type == "employee")
            {
                if (DatabaseContext.GetDB.Employee.Where(x => x.EmployeeCode == username
                && x.Email == email
                && x.PhoneNumber == phonenumber)
                    .FirstOrDefault() != null)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                var student = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == username).FirstOrDefault();
                if (student != null)
                {
                    var listContact = DatabaseContext.GetDB.Guardian.Where(x =>
                    x.StudentId == student.StudentId).ToList();

                    foreach (var contact in listContact)
                    {
                        if (contact.PhoneNumber == phonenumber)
                        {
                            if (contact.Email == email)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// cập nhật mật khẩu mới
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="password">Mật khẩu mới</param>
       
        public void UpdatePassword(string username, string password)
        {
            var account = DatabaseContext.GetDB.Account.Where(x => x.AccountCode == username).FirstOrDefault();
            account.Password = password;
            DatabaseContext.GetDB.SaveChanges();
        }
    }
}
