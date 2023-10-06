using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.AccountDAL;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.AccountBAL
{
    public interface IAccountBAL
    {
        /// <summary>
        /// Kiểm tra thông tin đăng nhập và trạng thái hoạt động của tài khoản
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>trả về tk nếu khớp, ngược lại thì null</returns>
        public Account? CheckLoginInfor(string username, string password);
        /// <summary>
        /// Kiểm tra thông tin xác nhận
        /// </summary>
        /// <param name="type">Loại tài khoản</param>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="email">email</param>
        /// <param name="phonenumber">số điện thoại</param>
        /// <returns>true nếu đúng, false nếu toàn bộ thông tin sai</returns>
    
        public string? CheckInformationForgot(string type, string username, string email, string phonenumber);

        /// <summary>
        /// cập nhật mật khẩu mới
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="code">Mã xác nhận người dùng nhập</param>
        /// <param name="codeServer">mã xác nhận đã gửi cho người dùng</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <param name="comfirmPassword">Mật khẩu xác nhận</param>
        /// <returns>Trả về tru nếu thành công</returns>
      
        public void UpdatePassword(string username, string code, string codeServer, string newPassword, string comfirmPassword);
        
    }
}
