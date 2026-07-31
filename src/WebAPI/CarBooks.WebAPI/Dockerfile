# Build context is src/WebAPI so the whole solution is available to the restore step.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
# Restoring only the API project graph keeps the Aspire AppHost out of the image build.
RUN dotnet restore CarBooks.WebAPI/CarBooks.WebAPI.csproj
RUN dotnet publish CarBooks.WebAPI/CarBooks.WebAPI.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "CarBooks.WebAPI.dll"]
