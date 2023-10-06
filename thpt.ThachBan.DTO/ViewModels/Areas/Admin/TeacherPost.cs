using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Admin
{
    public class TeacherPost
    {
        public string EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public string DateOfBirthDate { get; set; }
        public string DateOfBirthMonth { get; set; }
        public string DateOfBirthYear { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nation { get; set; }
        public string Address { get; set; }
        public string WorkStartedDate { get; set; }
        public string WorkStartedMonth { get; set; }
        public string WorkStartedYear { get; set; }
        public string WorkEndedDate { get; set; }
        public string WorkEndedMonth { get; set; }
        public string WorkEndedYear { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int TypeAcc { get; set; }
        public List<string>? listClass { get; set; }
        public List<string>? listSubject { get; set; }
        public List<string>? listDepartment { get; set; }
    }
}
