#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Teams/Teams.API/Teams.API.csproj", "Services/Teams/Teams.API/"]
COPY ["Services/Teams/Teams.Domain/Teams.Domain.csproj", "Services/Teams/Teams.Domain/"]
COPY ["Services/Shared/Shared.RabbitMQ/Shared.RabbitMQ.csproj", "Services/Shared/Shared.RabbitMQ/"]
COPY ["Services/Teams/Teams.Infrastructure/Teams.Infrastructure.csproj", "Services/Teams/Teams.Infrastructure/"]
RUN dotnet restore "Services/Teams/Teams.API/Teams.API.csproj"
COPY . .
WORKDIR "/src/Services/Teams/Teams.API"
RUN dotnet build "Teams.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teams.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teams.API.dll"]