## 服务注册与发现

## 引言

- 服务注册：简单理解，就是有一个注册中心，我们的每个服务实例启动时，都去注册中心注册一下，告诉注册中心我的地址，端口等信息。同样的服务实例要删除时，去注册中心删除一下，注册中心负责维护这些服务实例的信息。
- 服务发现：既然注册中心维护了各个服务实例的信息，那么客户端通过注册中心就很容易发现服务的变化了。

有了服务注册与发现，客户端就不用再去配置各个服务实例的地址，改为从注册中心统一获取。  
那注册中心又是怎么保证每个地址的可用状态呢，假如某个实例挂了怎么办呢？原则上挂掉的实例不应该被客户端获取到，所以就要提到：健康检查 。

- 健康检查：每个服务都需要提供一个用于健康检查的接口，该接口不具备业务功能。服务注册时把这个接口的地址也告诉注册中心，注册中心会定时调用这个接口来检测服务是否正常，如果不正常，则将它移除，这样就保证了服务的可用性。

## 常用的注册中心

- `Consul`

- `ZooKeeper`

- `etcd`

- `Eureka`

## Consul 的使用

#### 下载安装

官网地址：[https://www.consul.io/](https://www.consul.io/)

`API` 使用说明：[API](https://www.consul.io/api-docs)

使用说明：

1. 将下载后的文件解压到指定的目录下

2. 在根目录下运行 `cmd`

3. 输入 `consul agent -dev` 启动 `consul` 控制台并保持控制台开启

4. 本地网页中输入：[http://localhost:8500/](http://localhost:8500/)

#### C# 编程指导

- 编写 `Consul` 帮助类

```csharp
public static class ConsulHelper
{
        /// <summary>
        /// 服务注册到consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {
            var consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(configuration["ConsulSetting:ConsulAddress"]);
            });

            var registration = new AgentServiceRegistration()
            {
                //服务实例唯一标识
                ID = Guid.NewGuid().ToString(),
                //服务名
                Name = configuration["ConsulSetting:ServiceName"],
                //服务IP
                Address = configuration["ConsulSetting:ServiceIP"],
                //服务端口 因为要运行多个实例，端口不能在appsettings.json里配置，在docker容器运行时传入
                Port = int.Parse(configuration["ConsulSetting:ServicePort"]),
                Check = new AgentServiceCheck()
                {
                    //服务启动多久后注册
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    //健康检查时间间隔
                    Interval = TimeSpan.FromSeconds(10),
                    //健康检查地址
                    HTTP = $"http://{configuration["ConsulSetting:ServiceIP"]}:{configuration["ConsulSetting:ServicePort"]}{configuration["ConsulSetting:ServiceHealthCheck"]}",
                    //超时时间
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            //服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();

            //应用程序终止时，取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
}
```

- 更新配置文件 `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConsulSetting": {
    "ServiceName": "OrderService",
    "ServiceIP": "localhost",
    "ServiceHealthCheck": "/healthcheck",
    //注意，docker容器内部无法使用localhost访问宿主机器，如果是控制台启动的话就用localhost
    "ConsulAddress": "http://host.docker.internal:8500"
  }
}
```

- 健康检查接口 `HealthCheckController`

```csharp
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 健康检查接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
```

#### Docker 打包预发布

- 打包

```powershell
# 打包订单服务
docker build -t orderapi:1.0 -f ./OrderApi/Dockerfile .

# 打包产品服务
docker build -t productapi:1.0 -f ./ProductApi/Dockerfile .
```

- 发布

```powershell
# 发布订单服务 1、2、3、4、5
docker run -d -p 5001:80 --name orderservice1 orderapi:1.0 --ConsulSetting:ServicePort="5001"
docker run -d -p 5002:80 --name orderservice2 orderapi:1.0 --ConsulSetting:ServicePort="5002"
docker run -d -p 5003:80 --name orderservice3 orderapi:1.0 --ConsulSetting:ServicePort="5003"
docker run -d -p 5004:80 --name orderservice4 orderapi:1.0 --ConsulSetting:ServicePort="5004"
docker run -d -p 5005:80 --name orderservice5 orderapi:1.0 --ConsulSetting:ServicePort="5005"
# 发布产品服务 1、2、3、4、5
docker run -d -p 5006:80 --name productservice1 productapi:1.0 --ConsulSetting:ServicePort="5006"
docker run -d -p 5007:80 --name productservice2 productapi:1.0 --ConsulSetting:ServicePort="5007"
docker run -d -p 5008:80 --name productservice3 productapi:1.0 --ConsulSetting:ServicePort="5008"
docker run -d -p 5009:80 --name productservice4 productapi:1.0 --ConsulSetting:ServicePort="5009"
docker run -d -p 5010:80 --name productservice5 productapi:1.0 --ConsulSetting:ServicePort="5010"
```
