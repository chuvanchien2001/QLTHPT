using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Admin
{
    public class loadSubjectOfClassPost
    {
        public List<Guid> guids { get; set; }
        public Guid classId { get; set; }
    }
}
