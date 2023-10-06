using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thpt.ThachBan.BAL.StudentBAL;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Controllers
{
    public class AboutStudentController : Controller
    {
        #region Feild
        private IStudentBAL studentBAL;
        #endregion

        #region Contructor
        public AboutStudentController(IStudentBAL studentBAL)
        {
            this.studentBAL = studentBAL;
        }
        #endregion
        public IActionResult Index()
        {
            return View(studentBAL.GetAboutStudent(SessionManager.GetAccountCode(HttpContext)));
        }

    }
}
