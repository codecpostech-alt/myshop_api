using Microsoft.EntityFrameworkCore;
using MyShop.Data;
using MyShop.Services; // ← مهم حتى يتعرف على ChargilyPaymentService

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. قاعدة البيانات PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ 2. إعداد خدمة الدفع Chargily Pay
builder.Services.AddHttpClient<ChargilyPaymentService>();
builder.Services.AddScoped<ChargilyPaymentService>();

// ✅ 3. دعم MVC
builder.Services.AddControllersWithViews();

// ✅ 4. دعم البريد الإلكتروني
builder.Services.AddTransient<EmailService>();

// ✅ 5. دعم Session لتخزين اسم المستخدم
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ✅ 6. دعم HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ✅ 7. البيئة
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ 8. البنية الأساسية
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session
app.UseSession();

app.UseAuthorization();

// ✅ 9. المسارات
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();