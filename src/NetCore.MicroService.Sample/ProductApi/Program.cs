using ProductApi.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �������
IConfiguration _configuration = builder.Configuration;

var app = builder.Build();
IHostApplicationLifetime _leftime = app.Services.GetService<IHostApplicationLifetime>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//����ע��
app.RegisterConsul(_configuration, _leftime);

app.Run();
