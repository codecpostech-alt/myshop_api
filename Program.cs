using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using MyShop.Data;
using MyShop.Services;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// =======================================
// 🔐 1. Data Protection (Fix Session on Render)
// =======================================
var keysPath = Path.Combine(builder.Environment.WebRootPath ?? "wwwroot", "keys");

if (!Directory.Exists(keysPath))
    Directory.CreateDirectory(keysPath);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("MyShopApp");


// =======================================
// 🗄️ 2. Database PostgreSQL
// =======================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// =======================================
// 💳 3. Chargily Payment API
// =======================================
builder.Services.AddHttpClient<ChargilyPaymentService>();
builder.Services.AddScoped<ChargilyPaymentService>();


// =======================================
// 📧 4. Email Service
// =======================================
builder.Services.AddTransient<EmailService>();


// =======================================
// 🔐 5. Session
// =======================================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// =======================================
// 🌐 6. MVC + Controllers
// =======================================
builder.Services.AddControllersWithViews();


// =======================================
// 🚀 Build
// =======================================
var app = builder.Build();


// =======================================
// 🛡️ 7. Environment
// =======================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// =======================================
// ⚙️ 8. Middleware
// =======================================
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// ⚠️⚠️ VERY IMPORTANT — Enables API routes ⚠️⚠️
app.MapControllers();


// =======================================
// 🌍 9. MVC Default Route
// =======================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// =======================================
// ▶️ Run App
// =======================================
app.Run();
