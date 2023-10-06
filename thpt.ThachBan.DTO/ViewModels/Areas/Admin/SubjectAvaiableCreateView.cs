using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Admin
{
    public class SubjectAvaiableCreateView
    {
        public Subject subject { get; set; }
        public int inventory;
        public Employee Employee { get; set; }
        public List<int> x = new List<int>();
        public List<int> y = new List<int>();
    }
}
