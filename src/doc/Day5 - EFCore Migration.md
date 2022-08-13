# NET6 PostgreSQL CodeFirst Sample

> 本例以 `NetCore WebApi Top-Level Program`  展开讲解
> 
> 顶级语句（ `Top-Level Program` ）会极大简化入口文件的内容和编程方式

## Nuget 安装相关包

```powershell
 Microsoft.EntityFrameworkCore
 Microsoft.EntityFrameworkCore.Tools
 Npgsql.EntityFrameworkCore.PostgreSQL
```

## 编写相关实体类与数据库上下文

- 实体类

```csharp
[Table("Product", Schema = "public")]
public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    /// <summary>
    /// 产品名称
    /// </summary>
    [Required]
    [Column(TypeName = "VARCHAR(16)")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 库存
    /// </summary>
    [Required]
    public int Stock { get; set; }
}
```

- 数据库上下文

```csharp
public class ProductContext : DbContext
{
    public ProductContext() : base()
    {

    }
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
```

## 配置数据库连接

修改配置文件，增加数据库连接字符串

```json
"ConnectionStrings": {
  "ProductContext": "User ID=postgres;Password=foo;Host=host.docker.internal;Port=5432;Database=Product;Pooling=true;"
}
```

## 配置主程序

> **说明**
> 
> 现在 `WebApplication.CreateBuilder()` 方法，会创建一个 `WebApplicationBuilder` 类型的对象。
> 
> 此对象中有：
> 
> `builder.Services` - 对应原 `ConfigureServices` 方法
> 
> `builder.Configuration` - 老版本 `Ioc` 进来的配置对象
> 
> `builder.Environment` - 环境对象
> 
> 配置好 `builder` 后，`build` 一个 `WebApplication` 对象，即可对管道进行配置，一如老版本 `Configure` 方法

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ProductContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("ProductContext")));

var app = builder.Build();

// 代码实现数据库迁移
//using (var scope = app.Services.CreateScope())
//{
//    var dataContext = scope.ServiceProvider.GetRequiredService<ProductContext>();
//    dataContext.Database.Migrate();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

```

## 开始迁移数据库

1. 创建初始 `Migration` ，执行后项目目录下会产生 `Migrations` 文件夹和两个文件 

```powershell
# Visual Studio
Add-Migration InitialCreate

# Net Core CLI
dotnet ef migrations add InitialCreate
```

2. 更新 `Migration`

```powershell
# Visual Studio
Update-Database

# Net Core CLI
dotnet ef database update
```

## Enjoy :tada:


