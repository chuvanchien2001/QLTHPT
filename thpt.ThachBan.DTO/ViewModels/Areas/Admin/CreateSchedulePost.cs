using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thpt.ThachBan.DTO.ViewModels.Areas.Admin
{
    public class CreateSchedulePost
    {
        public Guid ClassId { get; set; }
        public List<Guid?> tiet1 { get; set; }
        public List<Guid?> tiet2 { get; set; }
        public List<Guid?> tiet3 { get; set; }
        public List<Guid?> tiet4 { get; set; }
        public List<Guid?> tiet5 { get; set; }
    }
}
