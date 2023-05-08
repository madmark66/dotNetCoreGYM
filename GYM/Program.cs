using GYM.Models;
using Microsoft.EntityFrameworkCore;

//=====================================
using Microsoft.AspNetCore.Authentication.Cookies;  // �ۤv�ʤ�[�J�]�ŧi�^ - �Х��w�� Nuget�M��
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//=====================================

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//=====================================================
// (1) �Х��w�� Nuget�M�� -- Microsoft.AspNetCore.Authentication.Cookies
// (2) �ۤv�ŧi Microsoft.AspNetCore.Authentication.Cookies; �R�W�Ŷ�
// (3) �ϥγo��� .AddAuthentication() �M .AddCookie() ��k�ӫإ����Ҥ����n��A��
//        VS2022 / .NET Core 6 �令  builder.Services...........
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                              .AddCookie(options => {
                                  // �H�U�o��ӳ]�w�i���i�L
                                  options.AccessDeniedPath = "/Test/AccessDeny";   // �ڵ��A�����\�n�J�A�|����o�@���C
                                  options.LoginPath = "/Test/Login";     // �n�J���C
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

//========================================= �]�H�U�O���I�^
app.UseAuthentication();   // �]���ҡ^�Цۤv�ʤ�[�J�I
                           // �]�w HttpContext.User �ݩʡA�ðw��n�D������v�����n��C
                           //=========================================     
app.UseAuthorization();   // �]���v�^

//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
