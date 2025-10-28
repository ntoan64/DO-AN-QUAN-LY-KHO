var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1?? ??ng k� c�c d?ch v? MVC, Session, HttpContextAccessor
// ============================================================
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

// ============================================================
// 2?? Build ?ng d?ng
// ============================================================
var app = builder.Build();

// ============================================================
// 3?? C?u h�nh pipeline HTTP
// ============================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Cho ph�p ph?c v? file t?nh (CSS, JS, h�nh ?nh)
app.UseStaticFiles();
app.UseSession();
// Routing MVC
app.UseRouting();

// S? d?ng Session
app.UseSession();

// �nh x? route m?c ??nh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ============================================================
// 4?? Ch?y ?ng d?ng
// ============================================================
app.Run();
