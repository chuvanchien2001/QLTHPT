using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using thpt.ThachBan.BAL.EmployeeBAL;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Common;

namespace thpt.ThachBan.v2.Areas.Teacher.Controllers
{
    public class AboutTeacherController : Controller
    {
        #region field
        private IEmployeeBAL employeeBAL;
        #endregion

        #region contructor
        public AboutTeacherController(IEmployeeBAL employeeBAL)
        {
            this.employeeBAL = employeeBAL;
        }
        #endregion
        public IActionResult Index()
        {
            dynamic data = JsonConvert.DeserializeObject(HttpContext.Session.GetString("UserInfor"));
            string code = data.AccountCode;
            
            return View(employeeBAL.GetAboutEmployee(code));
        }
    }
}
