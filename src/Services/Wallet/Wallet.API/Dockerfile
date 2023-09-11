#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Services/Wallet/Wallet.API/Wallet.API.csproj", "Services/Wallet/Wallet.API/"]
COPY ["Services/Wallet/Wallet.Core/Wallet.Core.csproj", "Services/Wallet/Wallet.Core/"]
COPY ["Services/Wallet/Wallet.Infrastructure/Wallet.Infrastructure.csproj", "Services/Wallet/Wallet.Infrastructure/"]
COPY ["BuildingBlocks/EventBus/EventBusKafka/EventBusKafka.csproj", "BuildingBlocks/EventBus/EventBusKafka/"]
RUN dotnet restore "Services/Wallet/Wallet.API/Wallet.API.csproj"
COPY . .
WORKDIR "/src/Services/Wallet/Wallet.API"
RUN dotnet build "Wallet.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wallet.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt update && apt install -y libc-dev
ENTRYPOINT ["dotnet", "Wallet.API.dll"]