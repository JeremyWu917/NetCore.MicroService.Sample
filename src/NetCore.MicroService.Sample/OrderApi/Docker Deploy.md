# 在 Docker 中运行服务

## Build 镜像
```cmd
docker build -t productapi -f ./ProductApi/Dockerfile .
docker build -t orderapi -f ./OrderApi/Dockerfile .
```

## 运行容器
```cmd
docker run -d -p 5000:80 --name productservice productapi
docker run -d -p 5001:80 --name orderservice orderapi
```
## 查看运行的容器
```cmd
docker ps
```

注意：需要在项目根目录下打开 `cmd`

