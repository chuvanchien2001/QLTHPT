using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.AccountDAL;
using thpt.ThachBan.DTO.Models;
using System.Text.RegularExpressions;

namespace thpt.ThachBan.BAL.AccountBAL
{
    public class AccountBAL:IAccountBAL
    {
        private IAccountDAL accountDAL;
        public AccountBAL(IAccountDAL accountDAL)
        {
            this.accountDAL = accountDAL;
        }
        /// <summary>
        /// Kiểm tra thông tin đăng nhập và trạng thái hoạt động của tài khoản
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>trả về tk nếu khớp, ngược lại thì null</returns>
        public Account? CheckLoginInfor(string username, string password)
        {
            return accountDAL.CheckLoginInfor(username, password);
        }

        /// <summary>
        /// Kiểm tra thông tin xác nhận
        /// </summary>
        /// <param name="type">Loại tài khoản</param>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="email">email</param>
        /// <param name="phonenumber">số điện thoại</param>
        /// <returns>mã Opt nếu đúng, null nếu sai</returns>
        public string? CheckInformationForgot(string type, string username, string email, string phonenumber)
        {
            if (!accountDAL.CheckInformationForgot(type, username, email, phonenumber))
            {
                return null;
            }
            else
            {
                return SendEmail(email);
            }
        }

        /// <summary>
        /// Gửi email
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>trả về Mã xác nhận</returns>

        public string SendEmail(string email)
        {
            string fromEmail = "chuvanchien2001barca@gmail.com"; // địa chỉ email của bạn
            string fromPassword = "xhzzltzbxseljqyr"; // mật khẩu email của bạn
            string toEmail = email; // địa chỉ email của người nhận
            string subject = "Mã OTP của bạn"; // chủ đề email
            string body = GenerateOTP(); // nội dung email, bao gồm mã OTP được tạo ra ngẫu nhiên

            // tạo đối tượng SmtpClient để kết nối đến máy chủ SMTP của Gmail
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(fromEmail, fromPassword);

            // tạo đối tượng MailMessage để gửi email
            MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            // gửi email
            client.Send(message);
            return body;
        }

        // phương thức để tạo mã OTP ngẫu nhiên
        public string GenerateOTP()
        {
            Random random = new Random();
            string otp = "";
            for (int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 9).ToString();
            }
            return otp;
        }

        /// <summary>
        /// cập nhật mật khẩu mới
        /// </summary>
        /// <param name="username">Mã/ tên đăng nhập</param>
        /// <param name="code">Mã xác nhận người dùng nhập</param>
        /// <param name="codeServer">mã xác nhận đã gửi cho người dùng</param>
        /// <param name="newPassword">Mật khẩu mới</param>
        /// <param name="comfirmPassword">Mật khẩu xác nhận</param>
        /// <returns>Trả về tru nếu thành công</returns>
 
        public void UpdatePassword(string username, string code, string codeServer, string newPassword, string comfirmPassword)
        {
            if (code != codeServer)
            {
                Exception missCode = new Exception("Mã code không đúng!");
                throw missCode;
            }
            if (newPassword != comfirmPassword)
            {
                Exception missPassword = new Exception("Xác nhận mật khẩu không khớp!");
                throw missPassword;
            }
            if (!CheckPassword(newPassword))
            {
                Exception validatePassword = new Exception("Mật khẩu cần ít nhất 8 kí tự, 1 chữ viết hoa, 1 số, 1 chữ thường!");
                throw validatePassword;
            }
            accountDAL.UpdatePassword(username, newPassword);

        }
        public bool CheckPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasNomalChar = new Regex(@"[a-z]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (password.Length < 8)
            {
                return false;
            }

            if (!hasNumber.IsMatch(password))
            {
                return false;
            }
            if (!hasNomalChar.IsMatch(password))
            {
                return false;
            }

            if (!hasUpperChar.IsMatch(password))
            {
                return false;
            }

            if (!hasSpecialChar.IsMatch(password))
            {
                return false;
            }

            return true;
        }
    }
}
