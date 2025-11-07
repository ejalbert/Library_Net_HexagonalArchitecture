FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/LibraryManagement.Web/LibraryManagement.Web/LibraryManagement.Web.csproj", "src/LibraryManagement.Web/LibraryManagement.Web/"]
COPY ["src/LibraryManagement.Web/LibraryManagement.Web.Client/LibraryManagement.Web.Client.csproj", "src/LibraryManagement.Web/LibraryManagement.Web.Client/"]
RUN dotnet restore "src/LibraryManagement.Web/LibraryManagement.Web/LibraryManagement.Web.csproj"
COPY . .
WORKDIR "/src/src/LibraryManagement.Web/LibraryManagement.Web"
RUN dotnet build "./LibraryManagement.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LibraryManagement.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryManagement.Web.dll"]
