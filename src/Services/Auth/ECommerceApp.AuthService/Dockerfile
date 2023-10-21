FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8009


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Auth/ECommerceApp.AuthService/ECommerceApp.AuthService.csproj", "src/Services/Auth/ECommerceApp.AuthService/"]
COPY ["src/Abstraction/Api/ECommerceApp.WebApiAbstraction/ECommerceApp.WebApiAbstraction.csproj", "src/Abstraction/Api/ECommerceApp.WebApiAbstraction/"]
COPY ["src/ECommerceApp.Core/ECommerceApp.Core.csproj", "src/ECommerceApp.Core/"]
COPY ["src/ECommerceApp.Application/ECommerceApp.Application.csproj", "src/ECommerceApp.Application/"]
COPY ["src/ECommerceApp.Infrastructure/ECommerceApp.Infrastructure.csproj", "src/ECommerceApp.Infrastructure/"]
RUN dotnet restore "src/Services/Auth/ECommerceApp.AuthService/ECommerceApp.AuthService.csproj"
COPY . .
WORKDIR "/src/src/Services/Auth/ECommerceApp.AuthService"
RUN dotnet build "ECommerceApp.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceApp.AuthService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApp.AuthService.dll"]