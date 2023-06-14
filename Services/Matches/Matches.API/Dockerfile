#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Matches/Matches.API/Matches.API.csproj", "Services/Matches/Matches.API/"]
COPY ["Services/Matches/Matches.Application/Matches.Application.csproj", "Services/Matches/Matches.Application/"]
COPY ["Services/Shared/Shared.RabbitMQ/Shared.RabbitMQ.csproj", "Services/Shared/Shared.RabbitMQ/"]
COPY ["Services/Matches/Matches.Domain/Matches.Domain.csproj", "Services/Matches/Matches.Domain/"]
COPY ["Services/Matches/Matches.Infrastructure/Matches.Infrastructure.csproj", "Services/Matches/Matches.Infrastructure/"]
RUN dotnet restore "Services/Matches/Matches.API/Matches.API.csproj"
COPY . .
WORKDIR "/src/Services/Matches/Matches.API"
RUN dotnet build "Matches.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Matches.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Matches.API.dll"]