using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.v2.Areas.Admin.Models
{
    public class ComboboxPickTeacher
    {
        public Subject subject { get; set; }
        public List<Employee> employees { get; set; }
        public int stt { get; set; }
    }
}
