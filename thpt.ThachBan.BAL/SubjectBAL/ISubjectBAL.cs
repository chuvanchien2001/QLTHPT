using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.SubjectBAL
{
    public interface ISubjectBAL
    {
        //public List<Subject> GetSubjectOfEmployee(Guid id);
        public List<Subject> GetSubjectByNameList(List<string> subjectNames);
    }
}
