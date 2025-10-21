##
## IMAGE NAME
## tanis/reformatributariaconsumo-api:0.08
##
## sudo docker build --pull --rm -f Dockerfile -t tanis/reformatributariaconsumo-api:0.01 . 
##

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./src/ReformaTributariaConsumo.API/ReformaTributariaConsumo.API.csproj", "ReformaTributariaConsumo.API/"]


RUN dotnet restore "./ReformaTributariaConsumo.API/ReformaTributariaConsumo.API.csproj"

COPY ./src/ ./
WORKDIR "/src/ReformaTributariaConsumo.API"
RUN dotnet build "./ReformaTributariaConsumo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReformaTributariaConsumo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReformaTributariaConsumo.API.dll"]
