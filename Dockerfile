# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and publish the app
COPY . .
RUN dotnet publish -c Release -o out

# Use the smaller ASP.NET runtime image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set environment variable for ASP.NET
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "MyAlumniApp.dll"]
