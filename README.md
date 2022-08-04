# NetCore.MicroService.Sample
:rocket: This is very cool MicroService using @NetCore Tech-Stacks.

## 构建镜像文件
```powershell
-- 构建一个名称为 webapp1 的镜像
-- 项目名称为 WebApplication1
docker build -t webapp1 -f ./WebApplication1/Dockerfile .
```

## 启动容器 
```powershell
-- 容器名称为 web1
docker run -d -p 5000:80 --name web1 webapp1
```

## Docker 常用命令
```powershell
-- 查看本地镜像
docker images
-- 查看运行中的容器
docker ps
```