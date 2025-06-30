# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5280
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CurrencyRatesDashboard.BlazoreUIss/CurrencyRatesDashboard.BlazoreUIss.csproj", "CurrencyRatesDashboard.BlazoreUIss/"]
COPY ["CurrencyRates.Application/CurrencyRates.Application.csproj", "CurrencyRates.Application/"]
COPY ["CurrencyRates.Domain/CurrencyRates.Domain.csproj", "CurrencyRates.Domain/"]
COPY ["CurrencyRates.Infrastructure/CurrencyRates.Infrastructure.csproj", "CurrencyRates.Infrastructure/"]
RUN dotnet restore "./CurrencyRatesDashboard.BlazoreUIss/CurrencyRatesDashboard.BlazoreUIss.csproj"
COPY . .
WORKDIR "/src/CurrencyRatesDashboard.BlazoreUIss"
RUN dotnet build "./CurrencyRatesDashboard.BlazoreUIss.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CurrencyRatesDashboard.BlazoreUIss.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyRatesDashboard.BlazoreUIss.dll"]