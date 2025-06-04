# Use the official ASP.NET runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY FollwUp.API.sln ./
COPY FollwUp.API/FollwUp.API.csproj FollwUp.API/

# Restore dependencies
RUN dotnet restore FollwUp.API.sln

# Copy everything else and build
COPY . .
WORKDIR /src/FollwUp.API
RUN dotnet publish -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FollwUp.API.dll"]
