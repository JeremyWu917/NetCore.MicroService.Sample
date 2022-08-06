# 在 Docker 中运行服务

## Build 镜像
```cmd
docker build -t productapi -f ./ProductApi/Dockerfile .
docker build -t orderapi -f ./OrderApi/Dockerfile .
```

## 运行容器
```cmd

docker run -d -p 5001:80 --name orderservice1 orderapi
docker run -d -p 5002:80 --name orderservice2 orderapi
docker run -d -p 5003:80 --name orderservice3 orderapi

docker run -d -p 5005:80 --name productservice1 productapi
docker run -d -p 5006:80 --name productservice2 productapi
docker run -d -p 5007:80 --name productservice3 productapi
```
## 查看运行的容器
```cmd
docker ps
```

注意：需要在项目根目录下打开 `cmd`

