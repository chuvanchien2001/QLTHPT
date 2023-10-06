using Microsoft.EntityFrameworkCore;
using thpt.ThachBan.DAL;
using thpt.ThachBan.DAL.AccountDAL;
using thpt.ThachBan.BAL;
using thpt.ThachBan.BAL.AccountBAL;
using thpt.ThachBan.DTO.Models;
using System.Text.Json.Serialization;
using thpt.ThachBan.v2.Models.Middleware;
using thpt.ThachBan.DAL.StudentDAL;
using thpt.ThachBan.BAL.StudentBAL;
using thpt.ThachBan.DAL.EmployeeDAL;
using thpt.ThachBan.BAL.EmployeeBAL;
using thpt.ThachBan.BAL.DepartmentBAL;
using thpt.ThachBan.BAL.SubjectBAL;
using thpt.ThachBan.DAL.DepartmentDAL;
using thpt.ThachBan.DAL.SubjectDAL;
using thpt.ThachBan.BAL.DepartmentManagerBAL;
using thpt.ThachBan.DAL.DepartmentManagerDAL;
using thpt.ThachBan.BAL.ClassBAL;
using thpt.ThachBan.DAL.ClassDAL;

namespace thpt.ThachBan.v2
{
    public class Program

    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAccountDAL, AccountDAL>();
            builder.Services.AddScoped<IAccountBAL,AccountBAL>();

            builder.Services.AddScoped<IStudentDAL,StudentDAL>();
            builder.Services.AddScoped<IStudentBAL, StudentBAL>();

            builder.Services.AddScoped<IEmployeeDAL, EmployeeDAL>();
            builder.Services.AddScoped<IEmployeeBAL, EmployeeBAL>();


            builder.Services.AddScoped<IDepartmentDAL, DepartmentDAL>();
            builder.Services.AddScoped<IDepartmentBAL, DepartmentBAL>();

            builder.Services.AddScoped<ISubjectDAL, SubjectDAL>();
            builder.Services.AddScoped<ISubjectBAL, SubjectBAL>();

            builder.Services.AddScoped<IDepartmentManagerDAL, DepartmentManagerDAL>();
            builder.Services.AddScoped<IDepartmentManagerBAL, DepartmentManagerBAL>();

            builder.Services.AddScoped<IClassDAL, ClassDAL>();
            builder.Services.AddScoped<IClassBAL, ClassBAL>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // dùng session bên trong view
            builder.Services.AddHttpContextAccessor();



            // middleware
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "user";
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            // chuyển thông tin đăng nhập thành json để lưu trữ trong session
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //Add services to DBContext
            DatabaseContext.ContextString = builder.Configuration.GetConnectionString("thpt.ThachBanContext");
            builder.Services.AddDbContext<thptThachBanContext>(options => {
                string connectstring = builder.Configuration.GetConnectionString("thpt.ThachBanContext");
                options.UseSqlServer(connectstring);
            });
            DatabaseContext.GetDB = DatabaseContext.SetDB();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // đăng ký session 
            app.UseSession();
            // bắt người dùng đăng nhập trước khi sử dụng hệ thống 
            app.UseMiddleware<CheckSessionMiddleware>();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "StudentArea",
                    areaName: "Student",
                    pattern: "Student/{controller=HomeStudent}/{action=Index}/{id?}",
                    defaults: new { area = "Student" }
                );
                endpoints.MapAreaControllerRoute(
                    name: "TeacherArea",
                    areaName: "Teacher",
                    pattern: "Teacher/{controller=HomeTeacher}/{action=Index}/{id?}",
                    defaults: new { area = "Teacher" }
                );
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "Admin/{controller=HomeAdmin}/{action=Index}/{id?}",
                    defaults: new { area = "Admin" }
                );
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Login}/{id?}"
                );
            });
            app.Run();
        }
    }
}