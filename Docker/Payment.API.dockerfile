FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

ENV PORT=8080


COPY *.csproj ./
RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE $PORT

ENTRYPOINT ["dotnet", "NET-Core-Web-API-Docker-Demo.dll"]