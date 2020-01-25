#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["ScooterBear.GTD/ScooterBear.GTD.csproj", "ScooterBear.GTD/"]
COPY ["ScooterBear.GTD.DynamoDb/ScooterBear.GTD.DynamoDb.csproj", "ScooterBear.GTD.DynamoDb/"]
COPY ["ScooterBear.GTD.Application/ScooterBear.GTD.Application.csproj", "ScooterBear.GTD.Application/"]
COPY ["ScooterBear.GTD.Patterns/ScooterBear.GTD.Patterns.csproj", "ScooterBear.GTD.Patterns/"]
RUN dotnet restore "ScooterBear.GTD/ScooterBear.GTD.csproj"
COPY . .
WORKDIR "/src/ScooterBear.GTD"
RUN dotnet build "ScooterBear.GTD.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScooterBear.GTD.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScooterBear.GTD.dll"]