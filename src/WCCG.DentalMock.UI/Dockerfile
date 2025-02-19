FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./WCCG.DentalMock.UI.csproj", "WCCG.DentalMock.UI/"]
RUN dotnet restore "./WCCG.DentalMock.UI/WCCG.DentalMock.UI.csproj"
COPY . "./WCCG.DentalMock.UI"
WORKDIR "/src/WCCG.DentalMock.UI"
RUN dotnet build "./WCCG.DentalMock.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WCCG.DentalMock.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WCCG.DentalMock.UI.dll"]
