FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY [ "src/BackgroundService/", "background.service/" ]
COPY [ "src/Common/", "Common/" ]

WORKDIR /src/background.service/Yarnique.BackgroundService
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Yarnique.BackgroundService.dll"]