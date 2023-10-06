using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Student
{
    public class AboutStudentPost
    {
        public Guid StudentId {get;set;} 
        public string StudentCode {get;set;}
        public string StudentName {get;set;}
        public int Gender {get;set;}
        public string DateOfBirthDate {get;set;}
        public string DateOfBirthMonth {get;set;}
        public string DateOfBirthYear {get;set;}
        public string PlaceOfBirth {get;set;}
        public string Nation {get;set;}
        public string Address {get;set;}
        public Guid Class {get;set;}
        public Guid StudentTask {get;set;}
        public Guid? SocialPolicy = null;
        public int Status { get; set; }

        public Guardian mother { get; set; } = new Guardian();
        public Guardian father { get; set; } = null;
        public Guardian other { get;set; } = new Guardian();

    }
}
