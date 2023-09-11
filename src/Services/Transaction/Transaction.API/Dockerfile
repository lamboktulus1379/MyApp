#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Services/Transaction/Transaction.API/Transaction.API.csproj", "Services/Transaction/Transaction.API/"]
COPY ["Services/Transactions/Transactions.Core/Transactions.Core.csproj", "Services/Transactions/Transactions.Core/"]
COPY ["Services/Transactions/Transactions.Usecase/Transactions.Usecase.csproj", "Services/Transactions/Transactions.Usecase/"]
COPY ["Services/Transactions/Transactions.Infrastructure/Transactions.Infrastructure.csproj", "Services/Transactions/Transactions.Infrastructure/"]
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