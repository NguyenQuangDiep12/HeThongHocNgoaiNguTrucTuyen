using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Services;
using HeThongHocNgoaiNguTrucTuyen.Services.Implementations;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            #region Cau hinh database
            builder.Services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    );
            });
            #endregion

            #region Cau hinh Cookie Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "ELearning.Auth.Cookie";
                    options.LoginPath = "/Auth/Login";
                    options.ExpireTimeSpan = TimeSpan.FromHours(1); // thời điểm hết hạn của authentication ticket mà ASP.NET Core sử dụng để xác thực người dùng còn xác thực hay không
                    options.SlidingExpiration = true; // Tham số SlidingExpiration được đặt thành true để hướng dẫn trình xử lý phát hành lại một cookie mới với thời gian hết hạn mới bất cứ khi nào nó xử lý một yêu cầu đã quá nửa thời gian hết hạn.
                    options.Cookie.MaxAge = options.ExpireTimeSpan; // Thời gian trình duyệt giữ cookie 
                    options.AccessDeniedPath = "/Auth/Forbidden";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    
                });
            #endregion

            #region DI
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ILanguageService, LanguageService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IVocabularyService, VocabularyService>();
            builder.Services.AddScoped<ITestService, TestService>();
            #endregion

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Language}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
