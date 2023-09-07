#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8008

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ECommerceApp.WebUI/ECommerceApp.WebUI.csproj", "src/ECommerceApp.WebUI/"]
COPY ["src/ECommerceApp.Application/ECommerceApp.Application.csproj", "src/ECommerceApp.Application/"]
COPY ["src/ECommerceApp.Core/ECommerceApp.Core.csproj", "src/ECommerceApp.Core/"]
COPY ["src/ECommerceApp.Infrastructure/ECommerceApp.Infrastructure.csproj", "src/ECommerceApp.Infrastructure/"]
RUN dotnet restore "src/ECommerceApp.WebUI/ECommerceApp.WebUI.csproj"
COPY . .
WORKDIR "/src/src/ECommerceApp.WebUI"
RUN dotnet build "ECommerceApp.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceApp.WebUI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApp.WebUI.dll"]