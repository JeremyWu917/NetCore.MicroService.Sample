using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ��� ocelot ����
builder.Services.AddOcelot();

// ���� json �ļ�
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// ���� ocelot �м��
app.UseOcelot().Wait();

app.Run();
