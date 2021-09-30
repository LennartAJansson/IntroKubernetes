#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SmhiApi/SmhiApi.csproj", "SmhiApi/"]
COPY ["WeatherContracts/WeatherContracts.csproj", "WeatherContracts/"]
COPY ["SmhiDb/SmhiDb.csproj", "SmhiDb/"]
RUN dotnet restore "SmhiApi/SmhiApi.csproj"
COPY . .
WORKDIR "/src/SmhiApi"
RUN dotnet build "SmhiApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmhiApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmhiApi.dll"]