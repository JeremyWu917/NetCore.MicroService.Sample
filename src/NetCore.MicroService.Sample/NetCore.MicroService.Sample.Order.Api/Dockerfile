#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NetCore.MicroService.Sample.Order.Api/NetCore.MicroService.Sample.Order.Api.csproj", "NetCore.MicroService.Sample.Order.Api/"]
RUN dotnet restore "NetCore.MicroService.Sample.Order.Api/NetCore.MicroService.Sample.Order.Api.csproj"
COPY . .
WORKDIR "/src/NetCore.MicroService.Sample.Order.Api"
RUN dotnet build "NetCore.MicroService.Sample.Order.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore.MicroService.Sample.Order.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore.MicroService.Sample.Order.Api.dll"]