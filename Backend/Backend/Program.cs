using Microsoft.EntityFrameworkCore;
using Backend.Data;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

//定義名為 "AllowVue" 的 CORS 通行證
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // 允許來自 Vue 的網址
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()   // 允許任何標頭
              .AllowAnyMethod();  // 允許 GET, POST, PUT, DELETE 等所有動作
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//把 DbContext 註冊進系統
//使AppDbContext成為連線到資料庫的橋樑，並且從 appsettings.json 的 ConnectionStrings 區塊讀取 DefaultConnection 的連線字串
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
