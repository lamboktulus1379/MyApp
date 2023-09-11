#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Services/Auth/Auth.API/Auth.API.csproj", "Services/Auth/Auth.API/"]
COPY ["Services/Auth/Auth.Core/Auth.Core.csproj", "Services/Auth/Auth.Core/"]
COPY ["Services/Auth/Auth.Usecase/Auth.Usecase.csproj", "Services/Auth/Auth.Usecase/"]
COPY ["Services/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "Services/Auth/Auth.Infrastructure/"]
COPY ["Services/Auth/TokenService/TokenService.csproj", "Services/Auth/Token.Service/"]
RUN dotnet restore "Services/Auth/Auth.API/Auth.API.csproj"
COPY . .
WORKDIR "/src/Services/Auth/Auth.API"
RUN dotnet build "Auth.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt update && apt install -y libc-dev
ENTRYPOINT ["dotnet", "Auth.API.dll"]