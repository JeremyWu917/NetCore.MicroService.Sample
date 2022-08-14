using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// �����Ȩ���
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            //IdentityServer��ַ
            options.Authority = "https://localhost:5001";
            //��ӦIdp��ApiResource��Name
            options.Audience = "api1";
            //��ʹ��https
            options.RequireHttpsMetadata = false;
        });

// ��Ӽ�Ȩ
builder.Services.AddAuthorization(options =>
{
    //���ڲ�����Ȩ
    options.AddPolicy("WeatherPolicy", builder =>
    {
        //�ͻ���Scope�а���api1.weather.scope���ܷ���
        builder.RequireScope("api1.weather.scope");
    });
    //���ڲ�����Ȩ
    options.AddPolicy("TestPolicy", builder =>
    {
        //�ͻ���Scope�а���api1.test.scope���ܷ���
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

//�����֤
app.UseAuthentication();

//��Ȩ
app.UseAuthorization();

//app.UseAuthorization();

app.MapControllers();

app.Run();
