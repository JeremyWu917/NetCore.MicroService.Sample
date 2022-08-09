using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// 添加 ocelot 服务
//builder.Services.AddOcelot();

// 添加 ocelet、consul 服务
builder.Services.AddOcelot().AddConsul();

// 加载 json 文件
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// 设置 ocelot 中间件
app.UseOcelot().Wait();

app.Run();
