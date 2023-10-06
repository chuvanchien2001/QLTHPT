using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Globalization;
using thpt.ThachBan.BAL.ClassBAL;
using thpt.ThachBan.BAL.DepartmentBAL;
using thpt.ThachBan.BAL.DepartmentManagerBAL;
using thpt.ThachBan.BAL.EmployeeBAL;
using thpt.ThachBan.BAL.StudentBAL;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels.Areas.Student;
using thpt.ThachBan.v2.Models.UnititiesModel;

namespace thpt.ThachBan.v2.Areas.Admin.Controllers
{
    public class StudentManagerController : Controller
    {
        #region field
        private IStudentBAL studentBAL;
        #endregion

        #region contructor
        public StudentManagerController(IStudentBAL studentBAL)
        {
            this.studentBAL = studentBAL;
        }
        #endregion
        [HttpGet]
        public IActionResult Index(
            int OrderBy = 0,
            int pageCurrent = 1,
            int size = 10,
            string? CodeSearch = "",
            string? NameSearch = "",
            string? ClassSearch = "",
            string? StudentTaskSearch = "",
            string? SocialPolicySearch = "",
            int Status = 1,
            Guid? id = null
            )
        {
            TempData["status"]= Status;
            TempData["PageCurrent"]= pageCurrent;
            TempData["Size"]= size;
            TempData["CodeSearch"]= CodeSearch;
            TempData["NameSearch"]= NameSearch;
            TempData["OrderBy"]= OrderBy;
            TempData["ClassSearch"]= ClassSearch;
            TempData["TaskSearch"]= StudentTaskSearch;
            TempData["PolicyName"]= SocialPolicySearch;

            if (id != null)
            {
                Student student = DatabaseContext.GetDB.Student.Find(id);
                student.Status = 0;
                student.UpdatedBy = SessionManager.GetId(HttpContext);
                student.UpdatedDate = DateTime.Now;
                Class _class = DatabaseContext.GetDB.Class.Find(student.ClassId);
                _class.NumOfMem= _class.NumOfMem-1;
                DatabaseContext.GetDB.Update(student);
                DatabaseContext.GetDB.Update(_class);
                DatabaseContext.GetDB.SaveChanges();
            }
            List<Student> students = DatabaseContext.GetDB.Student.Where(x => x.Status == Status).ToList();
            foreach (Student student in students)
            {
                student.Class = DatabaseContext.GetDB.Class.Find(student.ClassId);
                student.SocialPolicy = DatabaseContext.GetDB.SocialPolicy.Find(student.SocialPolicyId);
                student.StudentTask = DatabaseContext.GetDB.StudentTask.Find(student.StudentTaskId);
            }
            if (!String.IsNullOrEmpty(CodeSearch))
            {
                students = students.Where(x => x.StudentCode.Contains(CodeSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(NameSearch))
            {
                students = students.Where(x => x.StudentName.Contains(NameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(ClassSearch))
            {
                students = students.Where(x => x.Class.ClassName.Contains(ClassSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(StudentTaskSearch))
            {
                students = students.Where(x => x.StudentTask.StudentTaskName.Contains(StudentTaskSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(SocialPolicySearch))
            {
                students = students.Where(x => x.SocialPolicy.SocialPolicyName.Contains(SocialPolicySearch)).ToList();
            }

            if (OrderBy == 0)
            {
                students = students.OrderBy(x => x.StudentCode).ToList();
            }
            else if (OrderBy == 1)
            {
                students = students.OrderBy(x => x.CreatedDate).ToList();
            }
            else if (OrderBy == 2)
            {
                students = students.OrderBy(x => x.StudentName).ToList();
            }

            ViewBag.pageSize = (int)Math.Ceiling((double)students.Count / size);
            students = students.Skip((pageCurrent - 1) * size).Take(size).ToList();
            ViewBag.pageCurrent = pageCurrent;
            return View(students);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            AboutStudent student = studentBAL.GetAboutStudent(id);
            ViewBag.CreatedBy = DatabaseContext.GetDB.Account.Find(student.student.CreatedBy).AccountCode;
            ViewBag.UpdatedBy = student.student.UpdatedBy == null ? null : DatabaseContext.GetDB.Account.Find(student.student.UpdatedBy).AccountCode;
            TempData["Classes"] = DatabaseContext.GetDB.Class.OrderBy(x => x.ClassName).ToList();
            TempData["StudentTasks"] = DatabaseContext.GetDB.StudentTask.OrderBy(x => x.StudentTaskName).ToList();
            TempData["SocialPolicys"] = DatabaseContext.GetDB.SocialPolicy.OrderBy(x => x.SocialPolicyName).ToList();
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent([FromBody] AboutStudentPost aboutStudentPost)
        {
            Student student = DatabaseContext.GetDB.Student.Find(aboutStudentPost.StudentId);
            student.StudentName = aboutStudentPost.StudentName;
            student.Gender = aboutStudentPost.Gender;
            student.Status = aboutStudentPost.Status;
            student.PlaceOfBirth = aboutStudentPost.PlaceOfBirth;
            student.Nation = aboutStudentPost.Nation;
            student.Address = aboutStudentPost.Address;
            student.ClassId = aboutStudentPost.Class;
            student.SocialPolicyId = aboutStudentPost.SocialPolicy;
            student.StudentTaskId = aboutStudentPost.StudentTask;
            student.UpdatedBy = SessionManager.GetId(HttpContext);
            student.UpdatedDate = DateTime.Now;
            try
            {
                DateTime dateOfBirth = DateTime.ParseExact(
                    $"{aboutStudentPost.DateOfBirthMonth}/{aboutStudentPost.DateOfBirthDate}/{aboutStudentPost.DateOfBirthYear}",
                    "MM/dd/yyyy",
                    CultureInfo.InvariantCulture);
                student.DateOfBirth = dateOfBirth;
            } catch (Exception ex)
            {
                return Json(new
                {
                    status = 501,
                    message = "Ngày sinh không hợp lệ",
                });
            }
            DatabaseContext.GetDB.Update(student);
            try
            {
                AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.mother, 0);
                AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.father, 1);
                AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.other, 2);
            } catch (Exception ex)
            {
                return Json(new
                {
                    status = 501,
                    message = ex.Message,
                });
            }
            DatabaseContext.GetDB.SaveChanges();
            return Json(new
            {
                status = 200,
                message = "Cập nhật thành công!",
            });

        }
        public int AddGuardianByStudentAbout(Guid studentId, Guardian guardian, int relation)
        {
            // tạo phụ huynh
            if (guardian.GuardianId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                // phụ huynh bỏ trống
                if (String.IsNullOrEmpty(guardian.GuardianName) &&
                    String.IsNullOrEmpty(guardian.Career) &&
                    String.IsNullOrEmpty(guardian.PhoneNumber) &&
                    String.IsNullOrEmpty(guardian.Email))
                {
                    return 0;
                }
                try
                {
                    Guardian _guardian = new Guardian();
                    _guardian = guardian;
                    _guardian.GuardianId = Guid.NewGuid();
                    _guardian.Relation = relation;
                    _guardian.StudentId = studentId;
                    DatabaseContext.GetDB.Guardian.Add(_guardian);
                    DatabaseContext.GetDB.SaveChanges();
                    return 1;
                } catch (Exception ex)
                {
                    string mess = "";
                    if (relation == 0)
                    {
                        mess += "Cần nhập đủ thông tin mẹ!";
                    }
                    else if (relation == 1)
                    {
                        mess += "Cần nhập đủ thông tin cha!";
                    }
                    else
                    {
                        mess += "Cần nhập đủ thông tin giám hộ!";
                    }
                    Exception exception = new Exception(mess);
                    throw exception;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(guardian.GuardianName) &&
                    String.IsNullOrEmpty(guardian.Career) &&
                    String.IsNullOrEmpty(guardian.PhoneNumber) &&
                    String.IsNullOrEmpty(guardian.Email))
                {
                    Guardian guardianDelete = DatabaseContext.GetDB.Guardian.Find(guardian.GuardianId);
                    DatabaseContext.GetDB.Guardian.Remove(guardianDelete);
                    DatabaseContext.GetDB.SaveChanges();
                    return 0;
                }
                try
                {
                    Guardian _guardian = DatabaseContext.GetDB.Guardian.Find(guardian.GuardianId);
                    _guardian.GuardianName = guardian.GuardianName;
                    _guardian.Career = guardian.Career;
                    _guardian.PhoneNumber = guardian.PhoneNumber;
                    _guardian.Email = guardian.Email;
                    _guardian.Career = guardian.Career;
                    DatabaseContext.GetDB.Update(_guardian);
                    return 1;
                }
                catch (Exception ex)
                {
                    Exception exception = new Exception("Cần nhập đầy đủ thông tin của phụ huynh!");
                    throw exception;
                }
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.StudentCode = GetStudentCode();
            TempData["Classes"] = DatabaseContext.GetDB.Class.Where(x => x.NumOfMem < x.NumOfSeat).OrderBy(x => x.ClassName).ToList();
            TempData["SocialPolicys"] = DatabaseContext.GetDB.SocialPolicy.OrderBy(x => x.SocialPolicyName).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult CreatePost([FromBody] AboutStudentPost aboutStudentPost)
        {

            Student student = new Student();
            student.StudentId = Guid.NewGuid(); ;
            student.StudentName = aboutStudentPost.StudentName;
            student.StudentCode = aboutStudentPost.StudentCode;
            student.Gender = aboutStudentPost.Gender;
            student.Status = 1;
            student.PlaceOfBirth = aboutStudentPost.PlaceOfBirth;
            student.Nation = aboutStudentPost.Nation;
            student.Address = aboutStudentPost.Address;
            student.StudentTaskId = Guid.Parse("7a222ab9-0d22-471b-87df-f60143f6b648");
            student.SocialPolicyId = aboutStudentPost.SocialPolicy;
            student.ClassId = aboutStudentPost.Class;
            student.CreatedDate = DateTime.Now;
            student.CreatedBy = SessionManager.GetId(HttpContext);
            try
            {
                DateTime dateOfBirth = DateTime.ParseExact(
                    $"{aboutStudentPost.DateOfBirthMonth}/{aboutStudentPost.DateOfBirthDate}/{aboutStudentPost.DateOfBirthYear}",
                    "MM/dd/yyyy",
                    CultureInfo.InvariantCulture);
                student.DateOfBirth = dateOfBirth;
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 501,
                    message = "Ngày sinh không hợp lệ",
                });
            }
            DatabaseContext.GetDB.Student.Add(student);
            Class _class = DatabaseContext.GetDB.Class.Find(student.ClassId);
            _class.NumOfMem = _class.NumOfMem + 1;
            DatabaseContext.GetDB.Class.Update(_class);

            DatabaseContext.GetDB.SaveChanges();
            CreateAccount(student.StudentCode, 2);
            try
            {
                int numOfGuardian = 0;
                numOfGuardian += AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.mother, 0);
                numOfGuardian += AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.father, 1);
                numOfGuardian += AddGuardianByStudentAbout(student.StudentId, aboutStudentPost.other, 2);
                if (numOfGuardian == 0)
                {
                    return Json(new
                    {
                        status = 501,
                        message = "Cần thông tin của ít nhất 1 phụ huynh để liên lạc!",
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = 501,
                    message = ex.Message,
                });
            }
            DatabaseContext.GetDB.SaveChanges();

            return Json(new
            {
                status = 200,
                message = "Tạo thành công!",
            });
        }
        public string GetStudentCode()
        {
            string newCode = "";
            using (SqlConnection conn = new SqlConnection(DatabaseContext.ContextString))
            {
                conn.Open();
                using (var command = new SqlCommand("GetNewStudentCode", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newCode = reader.GetString(0);
                        }
                    }
                }
            }
            return newCode;
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            try
            {
                Student student = DatabaseContext.GetDB.Student.Find(id);
                List<Guardian> guis = DatabaseContext.GetDB.Guardian.Where(x=>x.StudentId==student.StudentId).ToList();
                DatabaseContext.GetDB.Guardian.RemoveRange(guis);
                DatabaseContext.GetDB.Student.Remove(student);
                DatabaseContext.GetDB.SaveChanges();
                return Json(new { status = 200, message = "Xóa thành công" });
            }catch(Exception ex)
            {
                return Json(new {status=501, message=ex.Message});
            }

        }

        public void CreateAccount(string code,int GroupRole)
        {
            Account account = new Account();
            account.AccountCode = code;
            account.Password= "Abc1234!";
            account.Status = 1;
            if(GroupRole== 2) { 
                account.RoleId = Guid.Parse("431cae83-1738-45c0-a3fc-13caf6472b6a");
            }
            DatabaseContext.GetDB.Account.Add(account);
            DatabaseContext.GetDB.SaveChanges();                
        }

    }
}
