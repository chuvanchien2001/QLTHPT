using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using thpt.ThachBan.BAL.EmployeeBAL;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.SubModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Admin;
using thpt.ThachBan.v2.Controllers;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace bai01.Areas.Student.Controllers
{
    public class HomeStudentController : BaseAreaController
    {
        #region field
        private IEmployeeBAL employeeBAL;
        #endregion

        #region contructor
        public HomeStudentController(IEmployeeBAL employeeBAL)
        {
            this.employeeBAL = employeeBAL;
        }
        #endregion
        public IActionResult ViewSchedule()
        {
            var classId = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == SessionManager.GetAccountCode(HttpContext)).FirstOrDefault().ClassId;
            var classOfStudent= DatabaseContext.GetDB.Class.Find(classId);
            TempData["ClassId"] = classId;
            TempData["Grade"] = classOfStudent.Grade;
            TempData["ClassName"] = classOfStudent.ClassName;
            return View("ViewSchedule");
        }
        [HttpPost]
        public IActionResult loadSubjectOfClass([FromBody] loadSubjectOfClassPost loadSubjectOfClassPost)
        {
            // Khởi tạo ds các tiết học sẽ trả về
            List<SubjectAvaiableCreateView> subjectsView = new List<SubjectAvaiableCreateView>();

            // lớp học được tạo tkb
            Class classPoint = DatabaseContext.GetDB.Class.Find(loadSubjectOfClassPost.classId);

            //  danh sách các môn học mà lớp học có thể học
            List<Subject> subjects = DatabaseContext.GetDB.Subject.Where(x => x.SubjectName.Contains(classPoint.Grade.ToString())).ToList();

            // danh sách tkb hiện tại của lớp học
            List<Schedule> schedules = DatabaseContext.GetDB.Schedule.Where(x => x.ClassId == classPoint.ClassId).ToList();

            // lấy dữ liệu cho các tiết hịc sẽ trả về
            for (int i = 0; i < subjects.Count; i++)
            {
                SubjectAvaiableCreateView subjectView = new SubjectAvaiableCreateView();
                subjectView.subject = subjects[i];
                subjectView.inventory = subjects[i].LessonAweek;
                subjectsView.Add(subjectView);
            }
            for (int i = 0; i < schedules.Count; i++)
            {
                foreach (var sub in subjectsView)
                {
                    if (sub.subject.SubjectId == schedules[i].SubjectId)
                    {
                        sub.x.Add(schedules[i].Day);
                        sub.y.Add(schedules[i].ClassTime);
                        sub.Employee = DatabaseContext.GetDB.Employee.Find(schedules[i].EmpoyeeId);
                        sub.inventory--;
                    }
                }
            }
            subjectsView = subjectsView.OrderBy(x => x.subject.SubjectName).ToList();
            return Json(subjectsView.Select(x => new
            {
                id = x.subject.SubjectId,
                name = x.subject.SubjectName,
                teacher = x.Employee,
                sl = x.inventory,
                x = x.x,
                y = x.y
            }).ToList());
        }

        [HttpGet]
        public IActionResult InforClass()
        {
            var classId = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == SessionManager.GetAccountCode(HttpContext)).FirstOrDefault().ClassId;
            var classOfStudent = DatabaseContext.GetDB.Class.Find(classId);
            classOfStudent.Employee = DatabaseContext.GetDB.Employee.Find(classOfStudent.EmployeeId);
            return View("InforClass", classOfStudent);
        }

        [HttpPost]
        public IActionResult loadStudentOfClass(Guid id, int pageCurrent = 1, int size = 10, int OrderBy = 0)
        {
            List<thpt.ThachBan.DTO.Models.Student> students = DatabaseContext.GetDB.Student.Where(x => x.ClassId == id && x.Status == 1).ToList();
            foreach (thpt.ThachBan.DTO.Models.Student student in students)
            {
                student.StudentTask = DatabaseContext.GetDB.StudentTask.Find(student.StudentTaskId);
            }
            if (OrderBy == 0)
            {
                students.Sort(new StudentNameComparer());
            }
            if (OrderBy == 1)
            {
                students = students.OrderBy(x => x.CodeOfSeat).ToList();
            }
            int pageSize = (int)Math.Ceiling((double)students.Count / size);
            students = students.Skip((pageCurrent - 1) * size).Take(size).ToList();
            return Json(new
            {
                students = students,
                pageSize = pageSize,
                pageCurrent = pageCurrent,
                orderBy = OrderBy
            });
        }
        [HttpGet]
        public IActionResult GetLeaderOfClass()
        {
            var classId = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == SessionManager.GetAccountCode(HttpContext)).FirstOrDefault().ClassId;
            var classOfStudent = DatabaseContext.GetDB.Class.Find(classId);
            Employee teacher = DatabaseContext.GetDB.Employee.Find(classOfStudent.EmployeeId);

            return View("InforLeaderOfClass",employeeBAL.GetAboutEmployee(teacher.EmployeeCode));
        }
        public IActionResult GetResult()
        {
            return View("GetResult");
        }
        [HttpPost]
        public IActionResult GetResultByGrade(int grade)
        {
            List<Result> results = DatabaseContext.GetDB.Result.Where(
                x => x.StudentId ==
                DatabaseContext.GetDB.Student.Where(x => x.StudentCode == SessionManager.GetAccountCode(HttpContext)).FirstOrDefault().StudentId
                && x.Grade==grade)
                .ToList();
            foreach(Result result in results)
            {
                result.Subject=DatabaseContext.GetDB.Subject.Find(result.SubjectId);
            }
            return View("GetResultByGrade", results);
        }
    }
}
