using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WorkoutDiaryMVC.Data;
using WorkoutDiaryMVC.Models;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var cultureInfo = new CultureInfo("hr-HR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workout}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "calendar",
    pattern: "Calendar",
    defaults: new { controller = "Workout", action = "Calendar" }
);

app.Run();
