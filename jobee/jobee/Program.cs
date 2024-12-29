using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using jobee.Data;
using jobee.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure authentication with cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";  // Redirect to login if not authenticated
        options.AccessDeniedPath = "/User/AccessDenied";  // Redirect if access is denied
        options.SlidingExpiration = true;  // Extend session on activity
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Expire after 30 minutes of inactivity
    });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Enable session management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Session timeout
    options.Cookie.HttpOnly = true;  // Mark cookie as HttpOnly for security
});

var app = builder.Build();

// Configure middleware
app.UseStaticFiles(); // Serve static files
app.UseRouting();     // Enable routing

app.UseAuthentication();  // Enable authentication middleware
app.UseAuthorization();   // Enable authorization middleware
app.UseSession();         // Enable session management

app.MapControllers();     // Map controller routes
app.MapDefaultControllerRoute(); // Default route

app.Run();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the HttpContextAccessor service
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRPolicy", policy => policy.RequireRole("HR"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

builder.Services.AddSession();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
