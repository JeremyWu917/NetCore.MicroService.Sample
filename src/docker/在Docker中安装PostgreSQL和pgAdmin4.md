# 在 Docker 中安装 PostgreSQL 和 pgAdmin4

> 默认目标机器已**安装并运行** `docker`



1. 拉取 `postgresql` 镜像

```powershell
docker pull postgres
```

2. 运行 `postgresql`

```powershell
docker run -d -p 5432:5432 --name postgresql -v pgdata:/var/lib/postgresql/data -e POSTGRES_PASSWORD=foo postgres
```

3. 拉取 `postgresql` 可视化工具 `pgadmin4`

```powershell
docker pull dpage/pgadmin4
```

4. 运行 `pgadmin4`

```powershell
docker run -d -p 5433:80 --name pgadmin4 -e PGADMIN_DEFAULT_EMAIL=foo@bar.com -e PGADMIN_DEFAULT_PASSWORD=123456 dpage/pgadmin4
```

5. 打开浏览器访问 `pgadmin4` 
   
   http://localhost:5433/

6. 登录
   
   | 用户名         | 密码  |
   | ----------- | --- |
   | foo@bar.com | foo |

7. Enjoy :tada:




