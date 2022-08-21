using WebMVC.Helper;

var builder = WebApplication.CreateBuilder(args);
// 添加鉴权中心
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    }).AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "http://apigateway:9000/auth";//通过网关访问鉴权中心
                                                         //options.Authority = "http://localhost:9080";

        options.ClientId = "jeremy";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.RequireHttpsMetadata = false;

        options.SaveTokens = true;

        options.Scope.Add("orderApiScope");
        options.Scope.Add("productApiScope");
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

// 注入IServiceHelper
//builder.Services.AddSingleton<IServiceHelper, ServiceHelper>();

//注入IServiceHelper 引入 ocelot
builder.Services.AddSingleton<IServiceHelper, GatewayServiceHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 定义变量
IServiceHelper serviceHelper = app.Services.GetService<IServiceHelper>();
// 程序启动时 获取服务列表
// 引入 ocelot 取消启动时获取服务列表
//serviceHelper.GetServices();

app.Run();
