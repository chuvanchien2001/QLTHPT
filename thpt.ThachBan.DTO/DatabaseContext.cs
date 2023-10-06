using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using thpt.ThachBan.DTO.Models;

namespace thpt.ThachBan.DAL
{
    public class DatabaseContext
    {
        public static string? ContextString;
        public static thptThachBanContext SetDB()
        {
            var optionsBuilder = new DbContextOptionsBuilder<thptThachBanContext>();
            optionsBuilder.UseSqlServer(ContextString);
            return new thptThachBanContext(optionsBuilder.Options);
        }
        public static thptThachBanContext GetDB;
    }
}