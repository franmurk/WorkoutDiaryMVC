using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
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

builder.Services.AddSingleton<WorkoutRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<WorkoutRepository>();

    Console.WriteLine(">>> Singleton repo count BEFORE: " + repo.GetAll().Count);
    
    Console.WriteLine(">>> Singleton repo count AFTER: " + repo.GetAll().Count);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workout}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "calendar",
    pattern: "Calendar",
    defaults: new { controller = "Workout", action = "Calendar" }
);


app.Run();
