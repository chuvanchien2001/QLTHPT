using Microsoft.AspNetCore.Mvc;
using thpt.ThachBan.BAL.AccountBAL;
using thpt.ThachBan.DAL;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using thpt.ThachBan.DTO.Models;
using NuGet.Protocol;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Controllers
{
    public class LoginController : Controller
    {
        #region Feild
        private IAccountBAL accountBAL;
        #endregion

        #region Contructor
        public LoginController(IAccountBAL accountBAL)
        {
            this.accountBAL = accountBAL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Điều hướng đến trang đăng nhập
        /// </summary>
        /// <returns>View Login và mess nếu thông tin sai, home page của quyền nếu đúng</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginPost(string username, string password)
        {
            Account acc = accountBAL.CheckLoginInfor(username, password);            
            if (acc == null)
            {
                ViewData["ErrorNotFound"] = "Thông tin đăng nhập sai!!!";
                ViewData["UserName"] = username;
                return View("Login");
            }
            else if (acc.Status == 0)
            {
                ViewData["ErrorNotFound"] = "Tài khoản đã bị khóa!!! Vui lòng liên hệ với Admin";
                return View("Login");
            }
            SessionManager.SetSessionInfor(acc, HttpContext);
            if (acc.Role.RoleGroup == 0)
            {
                return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
            }
            else if (acc.Role.RoleGroup == 1)
            {
                return RedirectToAction("Index", "HomeTeacher", new { area = "Teacher" });
            }
            return RedirectToAction("Index", "HomeStudent", new { area = "Student" });
        }

        ///<summary>
        ///Quên mật khẩu
        /// </summary>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(IFormCollection f)
        {
            var code = accountBAL.CheckInformationForgot(f["type"], f["username"], f["email"], f["numberphone"]);
            if (code != null)
            {
                TempData["username"] = f["username"].ToString();
                TempData["code"] = code;
                TempData.Keep("code");
                TempData.Keep("username");
                return View("ComfirmForgotPassword");
            }
            else
            {
                ViewData["Type"] = f["type"].ToString();
                ViewData["Username"] = f["username"].ToString();
                ViewData["Email"] = f["email"].ToString();
                ViewData["NumberPhone"] = f["numberphone"].ToString();
                ViewData["ErrorNotFound"] = "Thông tin xác thực không đúng!";
                return View("ForgotPassword");
            }
        }
        [HttpGet]
        public IActionResult ComfirmForgotPassword()
        {
            TempData.Keep("code");
            TempData.Keep("username");
            return View();
        }
        [HttpPost]
        public IActionResult ComfirmForgotPassword(IFormCollection f)
        {
            try
            {
                accountBAL.UpdatePassword(TempData["username"].ToString(), f["codeComfirm"], TempData["code"].ToString(), f["newPassword"], f["comfirmPassword"]);
            }
            catch (Exception ex)
            {
                ViewData["ErrorNotFound"] = ex.Message.ToString();
                TempData.Keep("code");
                TempData.Keep("username");
                return View();
            }
            ViewBag.UpdatePasswordSuccess = 1;
            return View("Login");
        }
        #endregion
    }
}