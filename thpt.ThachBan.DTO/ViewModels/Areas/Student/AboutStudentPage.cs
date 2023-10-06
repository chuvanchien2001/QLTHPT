using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Student
{
    public class AboutStudentPage
    {
        public Models.Student Student { get; set; }
        public string ClassName { get; set; }
        public string StudentTaskName { get; set; }
        public string SocialPolicyName { get; set; } 
    }
}
