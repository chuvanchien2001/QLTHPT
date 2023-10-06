using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL.SubjectDAL
{
    public class SubjectDAL:ISubjectDAL
    {
        //public List<Subject> GetSubjectOfEmployee(Guid id)
        //{
        //    return (from task in DatabaseContext.GetDB.TeacherTask.Where(x=>x.EmployeeId==id)
        //     join s in DatabaseContext.GetDB.Subject
        //     on task.SubjectId equals s.SubjectId
        //     select s).ToList();
        //}
        public List<Subject> GetSubjectByNameList(List<string> subjectNames)
        {
            List<Subject> subjects = new List<Subject>();
            foreach (string subjectName in subjectNames)
            {
                subjects.Add(DatabaseContext.GetDB.Subject.Where(x => x.SubjectName == subjectName).FirstOrDefault());

            }
            return subjects;
        }

    }
}
