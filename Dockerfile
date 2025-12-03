# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["bryx CRM/bryx CRM.csproj", "bryx CRM/"]
RUN dotnet restore "bryx CRM/bryx CRM.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/bryx CRM"
RUN dotnet build "bryx CRM.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "bryx CRM.csproj" -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install Entity Framework tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=5092

# Expose port
EXPOSE 5092

# Run the application
ENTRYPOINT ["dotnet", "bryx_CRM.dll"]
