# Docker Compose 与容器网络

## 什么是 Docker Compose ？

> Compose 是用于定义和运行多容器 Docker 应用程序的工具。通过 Compose 我们可以使用 YML 文件来配置应用程序需要的所有服务。然后使用一个命令，就可以从 YML 文件配置中创建并启动所有服务

简单来理解，`Compose` 类似一个批量工具，可以执行一组命令，支持批量构建镜像，批量启动容器，批量删除容器等等功能。

**注意 :warning:**

`Windows` 的 `Docker Desktop` 中已经包括了 `Compose` ，`Linux` 下 `Compose` 则需要单独安装一下

## YML

`yml` 文件是使用 `Compose` 必不可少的，在编写 `yml` 文件之前还需要准备 `Dockerfile` ，确保所有的项目都要添加 `Docker` 支持。

再在项目根目录下创建 `docker-compose.yml` 文件

以下是 `docker-compose.yml` 文件内容

```yml
version: '3.4' #Compose文件版本
services: #服务
    auth: #定义"Auth"服务 对应的是鉴权中心项目
        build: #构建
            context: . #构建上下文（目录）
            dockerfile: ./IDS4.AuthCenter/Dockerfile #Dockerfile文件目录
        ports: #端口
            - '5001:5001' #容器外部5001 容器内部5001
        environment: #环境变量
            - ASPNETCORE_URLS=https://+:5001#程序在容器内部https://+:5001运行 也可以写成http://0.0.0.0:5001
        networks: #容器网络
            - my-net #自定义网络my-net

    web: #定义"web"服务 对应的web客户端项目
        build: 
            context: .
            dockerfile: ./WebMVC/Dockerfile
        ports: 
            - '7146:7146'
        environment: 
            - ASPNETCORE_URLS=https://+:7146
        networks: 
            - my-net
        depends_on: #"web"服务依赖于"auth"服务和"apigateway"服务，此服务会在依赖服务之后执行
            - auth
            - apigateway

    apigateway: #定义"apigateway"服务 对应的网关项目
        build: 
            context: .
            dockerfile: ./OcelotApiGetway/Dockerfile
        ports: 
            - '9000:9000'
        environment: 
            - ASPNETCORE_URLS=https://+:9000
        networks:
            - my-net
        depends_on: 
            - orderapi1
            - orderapi2
            - orderapi3
            - productapi1
            - productapi2
            - productapi3

    productapi1: #定义"productapi1"服务 对应的产品服务项目
        image: productapi #指定镜像名称，如果不指定 默认是：netcoremicroservicedemo_productapi1，因为下面要用到所以指定一下
        build: 
            context: .
            dockerfile: ./ProductApi/Dockerfile
        ports: 
            - '9001:9001'
        environment: 
            - ASPNETCORE_URLS=https://+:9001
            - ConsulSetting:ServiceIP=productapi1 #程序参数
            - ConsulSetting:ServicePort=9001#程序参数
        networks: 
            - my-net
        depends_on: 
            - consul
            - postgres
            - rabbitmq
    productapi2:
        image: productapi #指定镜像名称为productapi，productapi1服务中已经构建了productapi镜像，所以不用重复构建
        ports: 
            - '9002:9002'
        environment: 
            - ASPNETCORE_URLS=https://+:9002
            - ConsulSetting:ServiceIP=productapi2
            - ConsulSetting:ServicePort=9002
        networks: 
            - my-net
        depends_on: 
            - productapi1
    productapi3:
        image: productapi
        ports: 
            - '9003:9003'
        environment: 
            - ASPNETCORE_URLS=https://+:9003
            - ConsulSetting:ServiceIP=productapi3 
            - ConsulSetting:ServicePort=9003
        networks: 
            - my-net
        depends_on: 
            - productapi1

    orderapi1:
        image: orderapi
        build: 
            context: .
            dockerfile: ./OrderApi/Dockerfile
        ports: 
            - '9005:9005'
        environment: 
            - ASPNETCORE_URLS=https://+:9005
            - ConsulSetting:ServiceIP=orderapi1
            - ConsulSetting:ServicePort=9005
        networks: 
            - my-net
        depends_on: 
            - consul
            - postgres
            - rabbitmq
    orderapi2:
        image: orderapi
        ports: 
            - '9006:9006'
        environment: 
            - ASPNETCORE_URLS=https://+:9006
            - ConsulSetting:ServiceIP=orderapi2
            - ConsulSetting:ServicePort=9006
        networks: 
            - my-net
        depends_on: 
            - orderapi1
    orderapi3:
        image: orderapi
        ports: 
            - '9007:9007'
        environment: 
            - ASPNETCORE_URLS=https://+:9007
            - ConsulSetting:ServiceIP=orderapi3
            - ConsulSetting:ServicePort=9007
        networks: 
            - my-net
        depends_on: 
            - orderapi1

    consul:
        image: consul #指定镜像名称为consul，本地如果没有consul镜像，会从docker远程仓库拉取
        ports: 
            - '8500:8500'
        networks: 
            - my-net

    postgres:
        image: postgres
        environment: 
            POSTGRES_PASSWORD: bar
        networks: 
            - my-net

    rabbitmq:
        image: rabbitmq
        networks: 
            - my-net

networks: #定义容器网络
    my-net: #my-net网络
        driver: bridge #网络模式为bridge


```

## 容器网络

默认情况下容器之间的通讯是比较麻烦的，之前是通过 `host.docker.internal` 或者容器的 `IP` 去访问，虽然是可以访问但有些不友好。更好的方式是，我们可以自定义一个 `bridge` 网络，将所有服务（容器）加入这个网络中，那么容器之间就可以直接通过服务名称通信了。

`bridge` 模式只是 `docker` 网络模式中的一种。


