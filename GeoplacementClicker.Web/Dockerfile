FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["GeoplacementClicker.Web/GeoplacementClicker.Web.csproj", "GeoplacementClicker.Web/"]
RUN dotnet restore "GeoplacementClicker.Web/GeoplacementClicker.Web.csproj"
COPY . .
WORKDIR "/src/GeoplacementClicker.Web"
RUN dotnet build "GeoplacementClicker.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "GeoplacementClicker.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GeoplacementClicker.Web.dll"]