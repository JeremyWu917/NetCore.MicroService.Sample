using IdentityServer4.AccessTokenValidation;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// 添加 ocelot 服务
//builder.Services.AddOcelot();

// 添加 ocelet、consul 服务
//builder.Services.AddOcelot().AddConsul();

// 添加ocelot、consul、cache 服务
//builder.Services.AddOcelot().AddConsul().AddCacheManager(x =>
//{
//    x.WithDictionaryHandle();
//});

// 添加统一鉴权中心支持
builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
        .AddIdentityServerAuthentication("orderService", options =>
        {
            options.Authority = "https://localhost:5001";//鉴权中心地址
            options.ApiName = "orderApi";
            options.SupportedTokens = SupportedTokens.Both;
            options.ApiSecret = "orderApi secret";
            options.RequireHttpsMetadata = false;
        })
        .AddIdentityServerAuthentication("productService", options =>
        {
            options.Authority = "https://localhost:5001";//鉴权中心地址
            options.ApiName = "productApi";
            options.SupportedTokens = SupportedTokens.Both;
            options.ApiSecret = "productApi secret";
            options.RequireHttpsMetadata = false;
        });

// 添加ocelot、consul、cache、polly 服务
builder.Services.AddOcelot().AddConsul().AddCacheManager(x =>
{
    x.WithDictionaryHandle();
}).AddPolly();

////添加consul支持
//.AddConsul()
////添加缓存
//.AddCacheManager(x =>
//{
//    x.WithDictionaryHandle();
//});

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
