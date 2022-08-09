using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// ��� ocelot ����
//builder.Services.AddOcelot();

// ��� ocelet��consul ����
builder.Services.AddOcelot().AddConsul();

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
