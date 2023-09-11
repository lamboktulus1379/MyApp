#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Services/Transaction/Transaction.API/Transaction.API.csproj", "Services/Transaction/Transaction.API/"]
COPY ["Services/Transaction/Transaction.Core/Transaction.Core.csproj", "Services/Transaction/Transaction.Core/"]
COPY ["Services/Transaction/Transaction.Usecase/Transaction.Usecase.csproj", "Services/Transaction/Transaction.Usecase/"]
COPY ["Services/Transaction/Transaction.Infrastructure/Transaction.Infrastructure.csproj", "Services/Transaction/Transaction.Infrastructure/"]
COPY ["BuildingBlocks/EventBus/EventBusKafka/EventBusKafka.csproj", "BuildingBlocks/EventBus/EventBusKafka/"]
RUN dotnet restore "Services/Transaction/Transaction.API/Transaction.API.csproj"
COPY . .
WORKDIR "/src/Services/Transaction/Transaction.API"
RUN dotnet build "Transaction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transaction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt update && apt install -y libc-dev
ENTRYPOINT ["dotnet", "Transaction.API.dll"]