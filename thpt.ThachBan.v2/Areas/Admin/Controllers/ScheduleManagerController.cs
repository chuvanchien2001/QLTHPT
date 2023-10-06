using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels.Areas.Admin;
using thpt.ThachBan.v2.Areas.Admin.Models;

namespace thpt.ThachBan.v2.Areas.Admin.Controllers
{
    public class ScheduleManagerController : Controller
    {
        [HttpGet]
        public IActionResult Create(Guid id)
        {
            TempData["ClassId"] = id;
            TempData["Grade"] = DatabaseContext.GetDB.Class.Find(id).Grade;
            return View();
        }
        
        [HttpPost]
        public IActionResult loadSubjectOfClass([FromBody] loadSubjectOfClassPost loadSubjectOfClassPost)
        {
            // Khởi tạo ds các tiết học sẽ trả về
            List<SubjectAvaiableCreateView> subjectsView = new List<SubjectAvaiableCreateView>();

            // lớp học được tạo tkb
            Class classPoint = DatabaseContext.GetDB.Class.Find(loadSubjectOfClassPost.classId);

            //  danh sách các môn học mà lớp học có thể học
            List<Subject> subjects = DatabaseContext.GetDB.Subject.Where(x=>x.SubjectName.Contains(classPoint.Grade.ToString())).ToList();

            // danh sách tkb hiện tại của lớp học
            List<Schedule> schedules = DatabaseContext.GetDB.Schedule.Where(x=>x.ClassId== classPoint.ClassId).ToList();

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
                foreach(var sub in subjectsView)
                {
                    if (sub.subject.SubjectId == schedules[i].SubjectId)
                    {
                        sub.x.Add(schedules[i].Day);
                        sub.y.Add(schedules[i].ClassTime);
                        sub.inventory--;
                    }
                }
            }
            subjectsView = subjectsView.OrderBy(x => x.subject.SubjectName).ToList();
            return Json(subjectsView.Select(x => new
            {
                id = x.subject.SubjectId,
                name = x.subject.SubjectName,
                sl = x.inventory,
                x = x.x,
                y = x.y
            }).ToList());
        }
        [HttpPost]
        public IActionResult loadTeacherHasExpertise(string grade)
        {
            List<Subject> subjects = DatabaseContext.GetDB.Subject.Where(x => x.SubjectName.Contains(grade)).Include(x=>x.Employee).ToList();
            List<ComboboxPickTeacher> comboboxPickTeachers = new List<ComboboxPickTeacher>();
            // cần tìm lần lượt các giáo viên dạy môn học tương ứng
            for(int i = 0; i < subjects.Count; i++)
            {
                ComboboxPickTeacher comboboxPickTeacher= new ComboboxPickTeacher();
                comboboxPickTeacher.employees = subjects[i].Employee.ToList();
                comboboxPickTeacher.stt = i;
                comboboxPickTeacher.subject = subjects[i];
                comboboxPickTeachers.Add(comboboxPickTeacher);
            }
            return Json(comboboxPickTeachers);
        }
        [HttpPost]
        public IActionResult CreateSchedule([FromBody] CreateSchedulePost createSchedulePost)
        {
            DatabaseContext.GetDB.Schedule.RemoveRange(DatabaseContext.GetDB.Schedule.Where(x=>x.ClassId==createSchedulePost.ClassId));
            DatabaseContext.GetDB.SaveChanges();
            for (int i = 0; i < 5; i++)//tiết
            {
                for (int j = 0; j < 6; j++)//ngày
                {
                    Schedule schedule = new Schedule();
                    schedule.ClassId = createSchedulePost.ClassId;
                    schedule.ClassTime = i;
                    schedule.Day = j;
                    if (i == 0) // tiết 1
                    {
                        schedule.SubjectId = createSchedulePost.tiet1[j];
                    }
                    else if (i == 1)
                    {
                        schedule.SubjectId = createSchedulePost.tiet2[j];
                    }
                    else if (i == 2)
                    {
                        schedule.SubjectId = createSchedulePost.tiet3[j];
                    }
                    else if (i == 3)
                    {
                        schedule.SubjectId = createSchedulePost.tiet4[j];
                    }
                    else if (i == 4)
                    {
                        schedule.SubjectId = createSchedulePost.tiet5[j];
                    }
                    DatabaseContext.GetDB.Schedule.Add(schedule);
                    DatabaseContext.GetDB.SaveChanges();
                }
            }
            return Json(new
            {
                status = 200,
            });
        }
    }
}
