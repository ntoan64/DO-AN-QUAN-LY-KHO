using QuanLyKho.API.Data;

var builder = WebApplication.CreateBuilder(args);

// 1?? Thêm d?ch v? Controller
builder.Services.AddControllers();

// 2?? C?u hình Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3?? ??ng ký c?u hình cho DatabaseContext n?u b?n có
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// 4?? B?t Swagger trong môi tr??ng dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5?? C?u hình pipeline HTTP
app.UseHttpsRedirection();
app.UseAuthorization();

// 6?? Kích ho?t Controller-based API
app.MapControllers();

// ? Xóa ph?n weatherforecast m?c ??nh vì b?n ?ã có controller riêng r?i
app.Run();
