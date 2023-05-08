using GYM.Models;
using Microsoft.EntityFrameworkCore;

//=====================================
using Microsoft.AspNetCore.Authentication.Cookies;  // 自己動手加入（宣告） - 請先安裝 Nuget套件
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//=====================================

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//=====================================================
// (1) 請先安裝 Nuget套件 -- Microsoft.AspNetCore.Authentication.Cookies
// (2) 自己宣告 Microsoft.AspNetCore.Authentication.Cookies; 命名空間
// (3) 使用這兩者 .AddAuthentication() 和 .AddCookie() 方法來建立驗證中介軟體服務
//        VS2022 / .NET Core 6 改成  builder.Services...........
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                              .AddCookie(options => {
                                  // 以下這兩個設定可有可無
                                  options.AccessDeniedPath = "/Test/AccessDeny";   // 拒絕，不允許登入，會跳到這一頁。
                                  options.LoginPath = "/Test/Login";     // 登入頁。
                              });


builder.Services.AddDbContext<GYMContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//========================================= （以下是重點）
app.UseAuthentication();   // （驗證）請自己動手加入！
                           // 設定 HttpContext.User 屬性，並針對要求執行授權中介軟體。
                           //=========================================     
app.UseAuthorization();   // （授權）

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
