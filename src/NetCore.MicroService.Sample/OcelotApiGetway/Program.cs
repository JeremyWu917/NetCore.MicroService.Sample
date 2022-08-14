using IdentityServer4.AccessTokenValidation;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// ��� ocelot ����
//builder.Services.AddOcelot();

// ��� ocelet��consul ����
//builder.Services.AddOcelot().AddConsul();

// ���ocelot��consul��cache ����
//builder.Services.AddOcelot().AddConsul().AddCacheManager(x =>
//{
//    x.WithDictionaryHandle();
//});

// ���ͳһ��Ȩ����֧��
builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
        .AddIdentityServerAuthentication("orderService", options =>
        {
            options.Authority = "https://localhost:5001";//��Ȩ���ĵ�ַ
            options.ApiName = "orderApi";
            options.SupportedTokens = SupportedTokens.Both;
            options.ApiSecret = "orderApi secret";
            options.RequireHttpsMetadata = false;
        })
        .AddIdentityServerAuthentication("productService", options =>
        {
            options.Authority = "https://localhost:5001";//��Ȩ���ĵ�ַ
            options.ApiName = "productApi";
            options.SupportedTokens = SupportedTokens.Both;
            options.ApiSecret = "productApi secret";
            options.RequireHttpsMetadata = false;
        });

// ���ocelot��consul��cache��polly ����
builder.Services.AddOcelot().AddConsul().AddCacheManager(x =>
{
    x.WithDictionaryHandle();
}).AddPolly();

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
