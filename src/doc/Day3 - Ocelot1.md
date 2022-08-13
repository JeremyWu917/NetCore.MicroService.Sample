# Ocelot Api 网关的应用

## 说明

1. 我们先暂时忽略 `Consul`，将服务实例的地址都写在配置文件中。要知道 `Consul`、`Ocelot` 等组件都是可以独立存在的。  

2. 配置文件中的 `Routes` 节点用来配置路由，`Downstream` 代表下游，也就是服务实例，`Upstream` 代表上游，也就是客户端。

3. 我们的路径比较简单，只有 `/products`、`/orders`，路径中如果有不固定参数则使用 `{}` 匹配。

4. 我们这个配置的意思呢就是客户端访问网关的 `/orders`、`/products`，网关会转发给服务实例的 `/orders`、`/products`，注意这个上游的路径不一定要和下游一致，比如上游路径可以配置成 `/api/orders`，`/xxx` 都可以。

5. `LoadBalancerOptions` 节点用来配置负载均衡，`Ocelot` 内置了 `LeastConnection`、`RoundRobin`、`NoLoadBalancer`、`CookieStickySessions`  `4` 种负载均衡策略。 

6. `BaseUrl` 节点就是配置我们 `ocelot` 网关将要运行的地址，注意此处的地址端口可能会发生变化，最好通过 `dotnet CLI` 指定断后启动 `Api` 服务。

## Ocelot 配置

> 创建 `ocelot.json` 文件，并配置属性复制到生成目录

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5006
        },
        {
          "Host": "localhost",
          "Port": 5007
        },
        {
          "Host": "localhost",
          "Port": 5008
        },
        {
          "Host": "localhost",
          "Port": 5009
        },
        {
          "Host": "localhost",
          "Port": 5010
        }
      ],
      "UpstreamPathTemplate": "/products",
      "UpstreamHttpMethod": [
        "Get"
      ],
      "LoadBalancerOptions": {
        //负载均衡，轮询机制 LeastConnection/RoundRobin/NoLoadBalancer/CookieStickySessions
        "Type": "RoundRobin"
      }
    },
    {
      "DownstreamPathTemplate": "/orders",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        },
        {
          "Host": "localhost",
          "Port": 5002
        },
        {
          "Host": "localhost",
          "Port": 5003
        },
        {
          "Host": "localhost",
          "Port": 5004
        },
        {
          "Host": "localhost",
          "Port": 5005
        }
      ],
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [
        "Get"
      ],
      "LoadBalancerOptions": {
        //负载均衡，轮询机制 LeastConnection/RoundRobin/NoLoadBalancer/CookieStickySessions
        "Type": "RoundRobin"
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:5239"
  }
}


```

## Program 改造一下

```csharp
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 添加 ocelot 服务
builder.Services.AddOcelot();

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

```


