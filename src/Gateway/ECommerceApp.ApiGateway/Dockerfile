FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8010


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gateway/ECommerceApp.ApiGateway/ECommerceApp.ApiGateway.csproj", "src/Gateway/ECommerceApp.ApiGateway/"]
RUN dotnet restore "src/Gateway/ECommerceApp.ApiGateway/ECommerceApp.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/ECommerceApp.ApiGateway"
RUN dotnet build "ECommerceApp.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceApp.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApp.ApiGateway.dll"]