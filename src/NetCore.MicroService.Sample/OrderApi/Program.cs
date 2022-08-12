using Microsoft.EntityFrameworkCore;
using OrderApi.Helper;
using OrderApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<OrderContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("OrderContext")));

//CAP
builder.Services.AddCap(x =>
{
    x.UseEntityFramework<OrderContext>();

    x.UseRabbitMQ("host.docker.internal");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 定义参数
IConfiguration _configuration = builder.Configuration;

var app = builder.Build();
IHostApplicationLifetime _leftime = app.Services.GetService<IHostApplicationLifetime>();

// 数据库迁移
//如果成功创建了数据库，则返回true
//orderContext.Database.EnsureCreated();
//orderContext.Database.Migrate();
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
    dataContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//服务注册
app.RegisterConsul(_configuration, _leftime);

app.Run();
