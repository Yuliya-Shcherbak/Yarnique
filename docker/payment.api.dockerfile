FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY [ "src/PaymentAPI/Yarnique.Payment.API/", "payment.api/" ]

WORKDIR /src/payment.api
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "Yarnique.Payment.API.dll"]