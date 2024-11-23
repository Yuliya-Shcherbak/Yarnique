FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY [ "src/API/", "API/" ]
COPY [ "src/Common/", "Common/" ]
COPY [ "src/Modules/", "Modules/" ]

WORKDIR /src/API/Yarnique.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Yarnique.API.dll"]