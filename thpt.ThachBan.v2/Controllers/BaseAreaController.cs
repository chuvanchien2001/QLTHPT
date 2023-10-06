using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thpt.ThachBan.BAL.AccountBAL;
using thpt.ThachBan.BAL.ClassBAL;
using thpt.ThachBan.BAL.DepartmentBAL;
using thpt.ThachBan.BAL.DepartmentManagerBAL;
using thpt.ThachBan.BAL.EmployeeBAL;
using thpt.ThachBan.BAL.SubjectBAL;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Controllers
{
    public class BaseAreaController : Controller
    {
        public IActionResult Index()
        {
            dynamic data = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserInfor"));
            string code = data.AccountCode.ToString();
            int role = data.Role.RoleGroup;
            if (role == 0 || role ==1)
            {
                TempData["Name"] = DatabaseContext.GetDB.Employee.Where(x => x.EmployeeCode == code).FirstOrDefault().EmployeeName;
            }
            else if (role== 2)
            {
                TempData["Name"] = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == code).FirstOrDefault().StudentName;
            }
            TempData["GroupRole"] = role;

            TempData.Keep("Name");
            TempData.Keep("GroupRole");
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserInfor");
            return Redirect("/Login");
        }
        public IActionResult ChangePass()
        {

            return View("ChangePass");
        }
        public IActionResult ChangePassPost(IFormCollection f)
        {
            string passOld = f["passOld"];
            string pass = f["pass"];
            string comfirmPass = f["comfirmPass"];
            Guid id = SessionManager.GetId(HttpContext);
            Account acc = DatabaseContext.GetDB.Account.Find(id);
            if (acc.Password.ToString() != passOld)
            {
                ViewBag.error = "Mật khẩu không đúng!!!";
            }
            else if (pass == comfirmPass)
            {
                acc.Password = pass;
                DatabaseContext.GetDB.Account.Update(acc);
                DatabaseContext.GetDB.SaveChanges();
                ViewBag.success = "Thành công!";
            }
            else
            {
                ViewBag.error = "Mật khẩu xác nhận không đúng!!!";
            }
            return View("ChangePass");
        }

    }
}
