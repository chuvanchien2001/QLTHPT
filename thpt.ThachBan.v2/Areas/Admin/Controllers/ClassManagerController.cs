using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.SubModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Admin;
using thpt.ThachBan.UI.Areas.Admin.Models;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Areas.Admin.Controllers
{
    public class ClassManagerController : Controller
    {

        public IActionResult Index(
            int Status = 1, int OrderBy = 0, int pageCurrent = 1, int size = 10, 
            string ClassNameSearch = null, 
            string NumOfMemSearch = null, 
            string NumOfSeatSearch = null,
            string GradeSearch =null,
            string EmployeeSearch =null
            )
        {
            ViewBag.ClassNameSearch=ClassNameSearch ;
            ViewBag.NumOfMemSearch=NumOfMemSearch ;
            ViewBag.NumOfSeatSearch=NumOfSeatSearch ;
            ViewBag.GradeSearch=GradeSearch ;
            ViewBag.EmployeeSearch = EmployeeSearch;
            ViewBag.Status = Status;
            List<AboutClass> aboutClasses = new List<AboutClass>();
            List<Class> classes;
            if (Status== 1)
            {
                classes = DatabaseContext.GetDB.Class.Where(x=>x.NumOfSeat- x.NumOfMem > 0).ToList();
            }
            else
            {
                classes = DatabaseContext.GetDB.Class.Where(x => x.NumOfSeat - x.NumOfMem <= 0).ToList();
            }
            if (!String.IsNullOrEmpty(ClassNameSearch))
            {
                classes = classes.Where(x => x.ClassName.Contains(ClassNameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(NumOfMemSearch))
            {
                classes = classes.Where(x => x.NumOfMem.ToString()==NumOfMemSearch).ToList();
            }
            if (!String.IsNullOrEmpty(NumOfSeatSearch))
            {
                classes = classes.Where(x => x.NumOfSeat.ToString() == NumOfSeatSearch).ToList();
            }
            if (!String.IsNullOrEmpty(GradeSearch))
            {
                classes = classes.Where(x => x.Grade.ToString()==GradeSearch).ToList();
            }
            if (!String.IsNullOrEmpty(EmployeeSearch))
            {
                var eNames = DatabaseContext.GetDB.Employee.Where(x => x.EmployeeName.Contains(EmployeeSearch)).Select(x=>x.EmployeeId)?.ToList();
                classes = (from e in eNames
                           join c in DatabaseContext.GetDB.Class
                           on e equals c.EmployeeId
                           select c).ToList();
            }
            for (int i = 0; i < classes.Count; i++)
            {
                AboutClass aboutClass = new AboutClass();
                aboutClass._Class = classes[i];
                aboutClass.EmployeeName = DatabaseContext.GetDB.Employee.Find(classes[i].EmployeeId)?.EmployeeName;
                aboutClasses.Add( aboutClass );
            }
            ViewBag.pageSize = (int)Math.Ceiling((double)aboutClasses.Count / size);
            aboutClasses = aboutClasses.Skip((pageCurrent - 1) * size).Take(size).ToList();
            ViewBag.pageCurrent = pageCurrent;
            return View(aboutClasses);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            ViewBag.Employees = DatabaseContext.GetDB.Employee.OrderBy(x=>x.EmployeeCode).ToList();
            Class Class = DatabaseContext.GetDB.Class.Find(id);

            if (Class != null)
            {
                Class.Employee = DatabaseContext.GetDB.Employee.Find(Class.EmployeeId);
            }
            ViewBag.UpdatedDate = DateTime.Now;
            ViewBag.UpdatedBy = SessionManager.GetAccountCode(HttpContext);
            ViewBag.CreatedBy = Class.Employee?.EmployeeCode;
            return View(Class);
        }

        [HttpPost]
        public IActionResult EditClass([FromBody] Class _class)
        {
            Class _Class = DatabaseContext.GetDB.Class.Find(_class.ClassId);
            _Class.ClassName = _class.ClassName;
            _Class.NumOfMem = _class.NumOfMem;
            _Class.NumOfSeat=_class.NumOfSeat;
            _Class.Grade=_class.Grade;
            _Class.EmployeeId=_class.EmployeeId;
            _Class.UpdatedDate = DateTime.Now;
            _Class.UpdatedBy= SessionManager.GetId(HttpContext);
           
            DatabaseContext.GetDB.Class.Update(_Class);
            DatabaseContext.GetDB.SaveChanges();
            return Json(new
            {
                status = 200,
                message = "Cập nhật thành công"
            }); ;
        }

        [HttpPost]
        public IActionResult loadStudentOfClass(Guid id, int pageCurrent = 1, int size = 10, int OrderBy = 0)
        {
            List<Student> students = DatabaseContext.GetDB.Student.Where(x => x.ClassId == id && x.Status == 1).ToList();
            foreach(Student student in students)
            {
                student.StudentTask = DatabaseContext.GetDB.StudentTask.Find(student.StudentTaskId);
            }
            if(OrderBy== 0)
            {
                students.Sort(new StudentNameComparer());
            }
            if (OrderBy == 1)
            {
                students=students.OrderBy(x=>x.CodeOfSeat).ToList();
            }
            int pageSize = (int)Math.Ceiling((double)students.Count / size);
            students = students.Skip((pageCurrent - 1) * size).Take(size).ToList();
            return Json(new
            {
                students = students,
                pageSize= pageSize,
                pageCurrent = pageCurrent,
                orderBy=OrderBy
            });
        }
    }
}
