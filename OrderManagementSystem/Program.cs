using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using OrderManagementSystem;
using OrderManagementSystem.CustomMiddleWare;
using OrderManagementSystem.Entity.Models;
using OrderManagementSystem.Entity.Security;
using OrderManagementSystem.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

String connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ConfigurationSettings.ConfigSettings(builder.Services, connectionString);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation().AddNToastNotifyToastr(new NToastNotify.ToastrOptions
{
    CloseButton = true,
    CloseDuration = true,
    TimeOut = 5000,
    PositionClass = ToastPositions.TopRight
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.UsePageNotFound();

app.Run();
