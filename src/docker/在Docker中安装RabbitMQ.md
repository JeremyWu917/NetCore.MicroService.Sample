# 在 Docker 中安装 RabbitMQ

在 `cmd` 中运行如下命令完成安装与部署

```powershell
docker pull rabbitmq:management
docker run -d -p 15672:15672 -p 5672:5672 --name rabbitmq rabbitmq:management
```

**注意**

默认用户：`guest`，密码：`guest`












