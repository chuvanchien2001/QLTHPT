using Microsoft.AspNetCore.Mvc;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels.Areas.Admin;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Areas.Admin.Controllers
{
    public class SubjectManagerController : Controller
    {
        [HttpGet]
        public IActionResult Index(
            int OrderBy = 0, int pageCurrent = 1, int size = 10,
            string? SubjectNameSearch = null,
            int? MaxLessonADaySearch = null,
            int? LessonAWeek = null,
            string? DepartmentSearch = null
            )
        {
            ViewBag.SubjectNameSearch = SubjectNameSearch ;
            ViewBag.MaxLessonADaySearch = MaxLessonADaySearch ;
            ViewBag.LessonAWeek = LessonAWeek ;
            ViewBag.DepartmentSearch = DepartmentSearch;
            List<Subject> subjects = DatabaseContext.GetDB.Subject.ToList();
            foreach( Subject subject in subjects )
            {
                subject.Department=DatabaseContext.GetDB.Department.Find(subject.DepartmentId);
            }
            if (!String.IsNullOrEmpty(SubjectNameSearch))
            {
                subjects = subjects.Where(x => x.SubjectName.Contains(SubjectNameSearch)).ToList();
            }
            if (MaxLessonADaySearch!=null)
            {
                subjects = subjects.Where(x => x.MaxLessonAday == MaxLessonADaySearch).ToList();
            }
            if (LessonAWeek.HasValue)
            {
                subjects = subjects.Where(x => x.LessonAweek == LessonAWeek).ToList();
            }
            if (!String.IsNullOrEmpty(DepartmentSearch))
            {
                subjects = subjects.Where(x => x.Department.DepartmentName.Contains(DepartmentSearch)).ToList();
            }
            ViewBag.pageSize = (int)Math.Ceiling((double)subjects.Count / size);
            subjects = subjects.Skip((pageCurrent - 1) * size).Take(size).ToList();
            ViewBag.pageCurrent = pageCurrent;
            return View(subjects);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            ViewBag.Departments = DatabaseContext.GetDB.Department.ToList();
            ViewBag.UpdatedBy = "";

            Subject subject = DatabaseContext.GetDB.Subject.Find(id);
            ViewBag.CreatedBy = DatabaseContext.GetDB.Account.Find(subject.CreatedBy);
            subject.Department = DatabaseContext.GetDB.Department.Find(subject.DepartmentId);
            return View(subject);
        }

        [HttpPost]
        public IActionResult EditSubject([FromBody]Subject subject)
        {
            Subject _sub = DatabaseContext.GetDB.Subject.Find(subject.SubjectId);
            _sub.SubjectName = subject.SubjectName;
            _sub.Description=subject.Description;
            _sub.MaxLessonAday= subject.MaxLessonAday;
            _sub.LessonAweek= subject.LessonAweek;
            _sub.DepartmentId= subject.DepartmentId;
            _sub.UpdatedBy = SessionManager.GetId(HttpContext);
            _sub.UpdatedDate = DateTime.Now;
            DatabaseContext.GetDB.Subject.Update(subject);
            DatabaseContext.GetDB.SaveChanges();
            return Json(new
            {
                status = 200,
                message = "Cập nhật thành công"
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = DatabaseContext.GetDB.Department.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateSubject([FromBody] Subject subject)
        {
            Subject _sub = new Subject();
            _sub.SubjectId= Guid.NewGuid();
            _sub.SubjectName = subject.SubjectName;
            _sub.Description = subject.Description;
            _sub.MaxLessonAday = subject.MaxLessonAday;
            _sub.LessonAweek = subject.LessonAweek;
            _sub.DepartmentId = subject.DepartmentId;
            _sub.CreatedBy = SessionManager.GetId(HttpContext);
            _sub.CreatedDate = DateTime.Now;
            DatabaseContext.GetDB.Subject.Update(subject);
            DatabaseContext.GetDB.SaveChanges();
            return Json(new
            {
                status = 200,
                message = "Tạo thành công"
            });
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                Subject subject = DatabaseContext.GetDB.Subject.Find(id);
                // check emlpoyee is null
                // delete data in TeacherExpertise

                DatabaseContext.GetDB.Subject.Remove(subject);
                DatabaseContext.GetDB.SaveChanges();
                return Json(new { status = 200, message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 501, message = ex.Message });
            }

        }
    }
}
