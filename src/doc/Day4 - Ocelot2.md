# Ocelot Api 网关 - 服务发现与治理

## 服务发现

>  引用 `Ocelot.Provider.Consul` 实现服务的发现 

修改 `ocelot.json` 配置信息

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/products",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "ProductService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      }
    },
    {
      "DownstreamPathTemplate": "/orders",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "OrderService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:9070",
    "ServiceDiscoveryProvider": {
      "Scheme": "http",
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
    }
  }
}
```

**注意：** `ServiceDiscoveryProvider` 节点需要定义 `consul` 实例的地址，`consul` 需要处于启动状态。

## 服务治理

### 缓存

> 引用 `Ocelot.Cache.CacheManager` 

修改 `ocelot.json` 配置信息

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/products",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "ProductService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      }
    },
    {
      "DownstreamPathTemplate": "/orders",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "OrderService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:9070",
    "ServiceDiscoveryProvider": {
      "Scheme": "http",
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
    }
  }
}

```

**注意：** 在 `Routers` 路由节点配置了 `FileCacheOptions` ，`TtlSeconds` 代表缓存的过期时间，`Region` 代表缓冲区名称。

### 限流

> 限流就是限制客户端一定时间内的请求次数

修改 `ocelot.json` 配置信息

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/products",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "ProductService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      },
      "RateLimitOptions": {
        "ClientWhitelist": [ "SuperClient" ],
        "EnableRateLimiting": true,
        "Period": "5s",
        "PeriodTimespan": 2,
        "Limit": 1
      }
    },
    {
      "DownstreamPathTemplate": "/orders",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "OrderService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      },
      "RateLimitOptions": {
        "ClientWhitelist": [ "SuperClient" ],
        "EnableRateLimiting": true,
        "Period": "5s",
        "PeriodTimespan": 2,
        "Limit": 2
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:9070",
    "ServiceDiscoveryProvider": {
      "Scheme": "http",
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
    },
    "RateLimitOptions": {
      "DisableRateLimitHeaders": false,
      "QuotaExceededMessage": "too many requests...",
      "HttpStatusCode": 999,
      "ClientIdHeader": "Test"
    }
  }
}
```

**注意**

1. 在 `Routes` 路由配置中增加了 `RateLimitOptions` 配置节点

2. `DisableRateLimitHeaders` 表示是否禁用 `X-Rate-Limit` 和 `Retry-After` 标头

3. `QuotaExceededMessage` 表示达到上限时返回给客户端的消息

4. `HttpStatusCode` 表示请求达到上限时返回给客户端的 `HTTP` 状态代码

5. `ClientIdHeader` 可以允许自定义用于标识客户端的标头。默认情况下为 `ClientId`

### 超时/熔断

> 引用 `Ocelot.Provider.Polly` 实现超时和熔断

修改 `ocelot.json` 配置信息

```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/products",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/products",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "ProductService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      },
      "RateLimitOptions": {
        "ClientWhitelist": [ "SuperClient" ],
        "EnableRateLimiting": true,
        "Period": "5s",
        "PeriodTimespan": 2,
        "Limit": 1
      }
    },
    {
      "DownstreamPathTemplate": "/orders",
      "DownstreamScheme": "http",
      "UpstreamPathTemplate": "/orders",
      "UpstreamHttpMethod": [ "Get" ],
      "ServiceName": "OrderService",
      "LoadBalancerOptions": {
        "Type": "RoundRobin"
      },
      "FileCacheOptions": {
        "TtlSeconds": 5,
        "Region": "regionname"
      },
      "RateLimitOptions": {
        "ClientWhitelist": [ "SuperClient" ],
        "EnableRateLimiting": true,
        "Period": "5s",
        "PeriodTimespan": 2,
        "Limit": 2
      }
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:9070",
    "ServiceDiscoveryProvider": {
      "Scheme": "http",
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
    },
    "RateLimitOptions": {
      "DisableRateLimitHeaders": false,
      "QuotaExceededMessage": "too many requests...",
      "HttpStatusCode": 999,
      "ClientIdHeader": "Test"
    },
    "QoSOptions": {
        "ExceptionsAllowedBeforeBreaking": 3,
        "DurationOfBreak": 10000,
        "TimeoutValue": 5000
    }
  }
}


```

**注意**

1. `ExceptionsAllowedBeforeBreaking` 代表发生错误的次数

2. `DurationOfBreak` 代表熔断时间

3. `TimeoutValue` 代表超时时间

4. 这里的时间都是以毫秒作为单位


