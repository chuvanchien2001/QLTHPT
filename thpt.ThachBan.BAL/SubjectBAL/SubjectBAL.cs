using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DAL.DepartmentDAL;
using thpt.ThachBan.DAL.SubjectDAL;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.BAL.SubjectBAL
{
    public class SubjectBAL :ISubjectBAL
    {
        private ISubjectDAL _ISubjectDAL;
        public SubjectBAL(ISubjectDAL ISubjectDAL)
        {
            _ISubjectDAL = ISubjectDAL;
        }
        //public List<Subject> GetSubjectOfEmployee(Guid employeeId)
        //{
        //    return _ISubjectDAL.GetSubjectOfEmployee(employeeId);
        //}

        public List<Subject> GetSubjectByNameList(List<string> subjectNames)
        {
            return _ISubjectDAL.GetSubjectByNameList(subjectNames);
        }
    }
}
