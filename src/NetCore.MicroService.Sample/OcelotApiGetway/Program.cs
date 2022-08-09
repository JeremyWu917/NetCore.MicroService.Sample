using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// ��� ocelot ����
//builder.Services.AddOcelot();

// ��� ocelet��consul ����
//builder.Services.AddOcelot().AddConsul();

//���ocelot��consul��cache ����
builder.Services.AddOcelot().AddConsul().AddCacheManager(x =>
{
    x.WithDictionaryHandle();
});
    ////���consul֧��
    //.AddConsul()
    ////��ӻ���
    //.AddCacheManager(x =>
    //{
    //    x.WithDictionaryHandle();
    //});

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
