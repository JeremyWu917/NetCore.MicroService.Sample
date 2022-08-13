# NetCore.MicroService.Sample

> :rocket: This is very cool MicroService using @NetCore Tech-Stacks.

## Docker 命令

### 1. 构建镜像文件

```powershell
# 构建一个名称为 webapp1 的镜像
# 项目名称为 WebApplication1
docker build -t webapp1 -f ./WebApplication1/Dockerfile .
```

### 2. 启动容器

```powershell
# 容器名称为 web1
docker run -d -p 5000:80 --name web1 webapp1
```

### 3. 查看镜像与运行的容器

```powershell
# 查看本地镜像
docker images
# 查看运行中的容器
docker ps
```

## 目录结构及相关技术栈

1. 微服务小试牛刀
   
   1. `Docker` 部署 `Api` 服务集群
   
   2. `MVC` 客户端
   
   3. `RestSharp`

2. 服务注册与发现
   
   1. `Consul` 服务注册与发现
   
   2. `Blocking Queries` 阻塞请求

3. Ocelot Api 网关
   
   1. 引用 `Ocelot` 通过网关进行请求
   2. 引用 `Ocelot.Provider.Consul` 实现服务发现
   3. 引用 `Ocelot.Cache.CacheManager` 实现服务治理（缓存、限流、超时熔断）

4. EventBus 事件总线
   
   1. `RabbitMQ`
   
   2. `PostgreSQL`

## 开发及运行环境

- Microsoft Visual Studio Enterprise 2022 (64 位) - Current 版本 17.2.6

- Ubuntu 20.04.4 LTS

- Docker Desktop version 4.11.0 (83626)

## 特别感谢

- [Consul](https://github.com/PlayFab/consuldotnet)

- [Ocelot](https://github.com/ThreeMammals/Ocelot)

- [CAP](https://github.com/dotnetcore/CAP)

- [RestSharp](https://github.com/restsharp/RestSharp)

- [小黑在哪里 - 博客园](https://www.cnblogs.com/xhznl/p/13259036.html)

## 开源协议

<p>
<a href="LICENSE">MIT</a>
</p>