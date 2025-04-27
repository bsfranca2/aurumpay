FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["backend/Directory.Build.props", "backend/"]
COPY ["backend/Directory.Packages.props", "backend/"]
COPY ["backend/global.json", "backend/"]
COPY ["backend/src/Checkout.Api/Checkout.Api.csproj", "backend/src/Checkout.Api/"]
COPY ["backend/src/ServiceDefaults/ServiceDefaults.csproj", "backend/src/ServiceDefaults/"]
COPY ["backend/src/Application/Application.csproj", "backend/src/Application/"]
COPY ["backend/src/Domain/Domain.csproj", "backend/src/Domain/"]
COPY ["backend/src/Infrastructure.EntityFramework/Infrastructure.EntityFramework.csproj", "backend/src/Infrastructure.EntityFramework/"]

RUN dotnet restore "backend/src/Checkout.Api/Checkout.Api.csproj"

COPY ["backend/src/", "backend/src/"]

WORKDIR "/src/backend/src/Checkout.Api"
RUN dotnet build "Checkout.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Checkout.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AurumPay.Checkout.Api.dll"]