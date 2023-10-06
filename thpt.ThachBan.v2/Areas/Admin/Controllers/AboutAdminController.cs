using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thpt.ThachBan.BAL.EmployeeBAL;

namespace thpt.ThachBan.v2.Areas.Admin.Controllers
{
    public class AboutAdminController : Controller
    {
        #region field
        private IEmployeeBAL employeeBAL;
        #endregion

        #region contructor
        public AboutAdminController(IEmployeeBAL employeeBAL)
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
