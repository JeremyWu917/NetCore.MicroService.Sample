#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#使用asp.net core 6.0作为基础镜像，起一个别名为base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#设置容器的工作目录为/app
WORKDIR /app
#暴露80端口
EXPOSE 80
#暴露443端口
EXPOSE 443

#使用.net core sdk 6.0作为基础镜像，起一个别名为build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#设置容器的工作目录为/src
WORKDIR /src
#拷贝NetCore.MicroService.Sample.Client/NetCore.MicroService.Sample.Client.csproj项目文件到容器中的/src/NetCore.MicroService.Sample.Client/目录
COPY ["NetCore.MicroService.Sample.Client/NetCore.MicroService.Sample.Client.csproj", "NetCore.MicroService.Sample.Client/"]
#执行dotnet restore命令，相当于平时用vs还原nuget包
RUN dotnet restore "NetCore.MicroService.Sample.Client/NetCore.MicroService.Sample.Client.csproj"

#拷贝当前目录的文件到到容器的/src目录
COPY . .
#设置容器的工作目录为/src/NetCore.MicroService.Sample.Client
WORKDIR "/src/NetCore.MicroService.Sample.Client"
#执行dotnet build命令，相当于平时用vs生成项目。以Release模式生成到容器的/app/build目录
RUN dotnet build "NetCore.MicroService.Sample.Client.csproj" -c Release -o /app/build
#将上面的build(.net core sdk 6.0)作为基础镜像，又重命名为publish
FROM build AS publish
#执行dotnet publish命令，相当于平时用vs发布项目。以Release模式发布到容器的/app/publish目录
RUN dotnet publish "NetCore.MicroService.Sample.Client.csproj" -c Release -o /app/publish

#将上面的base(asp.net core 6.0)作为基础镜像，又重命名为final
FROM base AS final
#设置容器的工作目录为/app
WORKDIR /app
#拷贝/app/publish目录到当前工作目录
COPY --from=publish /app/publish .
#指定容器入口命令，容器启动时会运行dotnet NetCore.MicroService.Sample.Client.dll
ENTRYPOINT ["dotnet", "NetCore.MicroService.Sample.Client.dll"]