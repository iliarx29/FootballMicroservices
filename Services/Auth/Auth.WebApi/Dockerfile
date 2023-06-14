#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Auth/Auth.WebApi/Auth.WebApi.csproj", "Services/Auth/Auth.WebApi/"]
COPY ["Services/Auth/Auth.Application/Auth.Application.csproj", "Services/Auth/Auth.Application/"]
COPY ["Services/Auth/Auth.Domain/Auth.Domain.csproj", "Services/Auth/Auth.Domain/"]
COPY ["Services/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "Services/Auth/Auth.Infrastructure/"]
RUN dotnet restore "Services/Auth/Auth.WebApi/Auth.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Auth/Auth.WebApi"
RUN dotnet build "Auth.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auth.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.WebApi.dll"]