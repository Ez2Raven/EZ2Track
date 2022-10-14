FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Crawler.WebApi/Crawler.WebApi.csproj", "Crawler.WebApi/"]
RUN dotnet restore "Crawler.WebApi/Crawler.WebApi.csproj"
COPY . .
WORKDIR "/src/Crawler.WebApi"
RUN dotnet build "Crawler.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crawler.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawler.WebApi.dll"]
