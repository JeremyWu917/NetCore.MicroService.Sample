using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 添加授权相关
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            //IdentityServer地址
            options.Authority = "https://localhost:5001";
            //对应Idp中ApiResource的Name
            options.Audience = "api1";
            //不使用https
            options.RequireHttpsMetadata = false;
        });

// 添加鉴权
builder.Services.AddAuthorization(options =>
{
    //基于策略授权
    options.AddPolicy("WeatherPolicy", builder =>
    {
        //客户端Scope中包含api1.weather.scope才能访问
        builder.RequireScope("api1.weather.scope");
    });
    //基于策略授权
    options.AddPolicy("TestPolicy", builder =>
    {
        //客户端Scope中包含api1.test.scope才能访问
        builder.RequireScope("api1.test.scope");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//身份验证
app.UseAuthentication();

//授权
app.UseAuthorization();

//app.UseAuthorization();

app.MapControllers();

app.Run();
