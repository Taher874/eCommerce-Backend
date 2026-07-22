# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore
COPY eCommerce/eSim.csproj eCommerce/
RUN dotnet restore eCommerce/eSim.csproj

# Copy everything
COPY . .

# Publish
WORKDIR /src/eCommerce
RUN dotnet publish eSim.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 10000

ENTRYPOINT ["dotnet", "eSim.dll"]