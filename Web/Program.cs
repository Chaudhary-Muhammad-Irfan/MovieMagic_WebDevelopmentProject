using Application_Layer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Security.Claims;
using Web_Development_Project.Data;
using Web_Development_Project.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<CORE.Interface.IBookingRepository, Infra_Structure_Layer.Repositories.BookingRepository>();
builder.Services.AddScoped<CORE.Interface.IMovieRepository, Infra_Structure_Layer.Repositories.MovieRepository>();
builder.Services.AddScoped<CORE.Interface.IMovieShowsRepository, Infra_Structure_Layer.Repositories.MovieShowsRepository>();
builder.Services.AddScoped(typeof(CORE.Interface.IRepository<>), typeof(Infra_Structure_Layer.Repositories.GenericRepository<>));
builder.Services.AddScoped(typeof(GenericService<>));
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<MovieShowsService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AddNewFields>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

//------------------------------------------------------Session------------------------------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
//-------------------------------------------------------Session----------------------------------------


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CountryPolicy", policy =>
    {
        policy.RequireClaim(ClaimTypes.Country, "Pakistan");
    });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Email, "Admin@movieMagic.com"));
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//------------------------------------------------------Session------------------------------------------
app.UseSession();
//-------------------------------------------------------Session----------------------------------------

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<Infra_Structure_Layer.Hubs.BookingHub>("/bookingHub");
app.Run();