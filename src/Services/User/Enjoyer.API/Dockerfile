#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Services/User/Enjoyer.API/Enjoyer.API.csproj", "Services/User/Enjoyer.API/"]
RUN dotnet restore "Services/User/Enjoyer.API/Enjoyer.API.csproj"
COPY . .
WORKDIR "/src/Services/User/Enjoyer.API"
RUN dotnet build "Enjoyer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Enjoyer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt update && apt install -y libc-dev
ENTRYPOINT ["dotnet", "Enjoyer.API.dll"]