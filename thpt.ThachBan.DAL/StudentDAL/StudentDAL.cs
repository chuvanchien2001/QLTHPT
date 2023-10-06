using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;
using thpt.ThachBan.DTO.ViewModels;
using thpt.ThachBan.DTO.ViewModels.Areas.Common;
using thpt.ThachBan.DTO.ViewModels.Areas.Student;

namespace thpt.ThachBan.DAL.StudentDAL
{
    public class StudentDAL:IStudentDAL
    {
        /// <summary>
        /// lấy các thông tin trả về cho view AboutStudent
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Thông tin cho view AboutStudent</returns>
        public AboutStudent GetAboutStudent(string code)
        {
            AboutStudent aboutStudent = new AboutStudent();
            // lấy thông tin học sinh
            aboutStudent.student = DatabaseContext.GetDB.Student.Where(x => x.StudentCode == code).FirstOrDefault();
            return GetAbout(aboutStudent);
        }

        /// <summary>
        /// lấy các thông tin trả về cho view AboutStudent
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Thông tin cho view AboutStudent</returns>
        public AboutStudent GetAboutStudent(Guid id)
        {
            AboutStudent aboutStudent = new AboutStudent();
            // lấy thông tin học sinh
            aboutStudent.student = DatabaseContext.GetDB.Student.Find(id);            
            return GetAbout(aboutStudent);
        }

        /// <summary>
        /// lấy các thông tin trả về cho view AboutStudent
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Thông tin cho view AboutStudent</returns>
        public AboutStudent GetAbout(AboutStudent aboutStudent)
        {
            List<Guardian> studentContacts = DatabaseContext.GetDB.Guardian.Where(x => x.StudentId == aboutStudent.student.StudentId).ToList();
            // lấy lớp học
            aboutStudent.student.Class = DatabaseContext.GetDB.Class.Find(aboutStudent.student.ClassId);
            // lấy nhiệm vụ trong lớp
            aboutStudent.student.StudentTask = DatabaseContext.GetDB.StudentTask.Find(aboutStudent.student.StudentTaskId);
            // lấy trợ cấp xã hội
            aboutStudent.student.SocialPolicy = DatabaseContext.GetDB.SocialPolicy.Find(aboutStudent.student.SocialPolicyId);
            foreach (Guardian studentContact in studentContacts)
            {
                if (studentContact.Relation == 0)
                {
                    aboutStudent.mother = DatabaseContext.GetDB.Guardian.Find(studentContact.GuardianId);
                }
                else if (studentContact.Relation == 1)
                {
                    aboutStudent.father = DatabaseContext.GetDB.Guardian.Find(studentContact.GuardianId);
                }
                else
                {
                    aboutStudent.other = DatabaseContext.GetDB.Guardian.Find(studentContact.GuardianId);
                }
            }
            return aboutStudent;
        }

        //public List<AboutStudent> StudentPaging(string CodeSearch = null, string NameSearch = null, string ClassSearch = null, string TaskSearch = null, string PolicyName = null)
        //{
        //    List<AboutEmployeePage> aboutEmployees = new List<AboutEmployeePage>();
        //    foreach (var item in DatabaseContext.GetDB.Employee.ToList())
        //    {
        //        AboutEmployeePage aboutEmployee = GetAboutEmployee(item.EmployeeCode);
        //        aboutEmployees.Add(aboutEmployee);
        //    }
        //    aboutEmployees = aboutEmployees.ToList();
        //    if (!String.IsNullOrEmpty(CodeSearch))
        //    {
        //        aboutEmployees = aboutEmployees.Where(p => p.Employee.EmployeeCode.Contains(CodeSearch)).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(NameSearch))
        //    {
        //        aboutEmployees = aboutEmployees.Where(p => p.Employee.EmployeeName.ToLower().Contains(NameSearch.ToLower())).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(ClassSearch))
        //    {
        //        aboutEmployees = aboutEmployees.Where(p => String.Join(", ", p.Classes.Select(x => x.ClassName)).ToLower()
        //        .Contains(ClassSearch.ToLower())).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(SubjectSearch))
        //    {
        //        aboutEmployees = aboutEmployees.Where(p => String.Join(", ", p.Subjects.Select(x => x.SubjectName)).ToLower().Contains(SubjectSearch.ToLower())).ToList();
        //    }
        //    if (!String.IsNullOrEmpty(NumberPhoneSearch))
        //    {
        //        aboutEmployees = aboutEmployees.Where(p => p.Employee.PhoneNumber.Contains(NumberPhoneSearch)).ToList();
        //    }
        //}
    }
}
