FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ERP/ERP.csproj", "ERP/"]
COPY ["Service_ERP/Service_ERP.csproj", "Service_ERP/"]
COPY ["DAL_ERP/DAL_ERP.csproj", "DAL_ERP/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "ERP/ERP.csproj"
COPY . .
WORKDIR "/src/ERP"
RUN dotnet build "ERP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERP.dll"]
