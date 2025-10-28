var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1?? ??ng ký các d?ch v? MVC, Session, HttpContextAccessor
// ============================================================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

// ============================================================
// 2?? Build ?ng d?ng
// ============================================================
var app = builder.Build();

// ============================================================
// 3?? C?u hình pipeline HTTP
// ============================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Cho phép ph?c v? file t?nh (CSS, JS, hình ?nh)
app.UseStaticFiles();
app.UseSession();
// Routing MVC
app.UseRouting();

// S? d?ng Session
app.UseSession();

// Ánh x? route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================================
// 4?? Ch?y ?ng d?ng
// ============================================================
app.Run();
