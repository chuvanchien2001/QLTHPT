using thpt.ThachBan.DTO.Models;
namespace thpt.ThachBan.DTO.ViewModels.Areas.Student
{
    public class AboutStudent
    {
        public Models.Student student = new Models.Student();
        public Guardian father = new Guardian();
        public Guardian mother = new Guardian();
        public Guardian other = new Guardian();
    }
}
