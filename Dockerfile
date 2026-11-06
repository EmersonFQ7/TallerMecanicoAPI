
# Etapa 1: imagen base de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Etapa 2: compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TallerMecanicoAPI.csproj", "./"]

RUN dotnet restore "TallerMecanicoAPI.csproj"

COPY . .

RUN dotnet publish "TallerMecanicoAPI.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Etapa 3: ejecución final
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TallerMecanicoAPI.dll"]
