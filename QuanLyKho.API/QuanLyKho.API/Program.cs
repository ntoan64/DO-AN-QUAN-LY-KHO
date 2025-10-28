using QuanLyKho.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1?? Th�m d?ch v? Controller
builder.Services.AddControllers();

// 2?? C?u h�nh Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3?? ??ng k� c?u h�nh cho DatabaseContext n?u b?n c�
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// 4?? B?t Swagger trong m�i tr??ng dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5?? C?u h�nh pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();

// 6?? K�ch ho?t Controller-based API
app.MapControllers();

// ? X�a ph?n weatherforecast m?c ??nh v� b?n ?� c� controller ri�ng r?i
app.Run();
