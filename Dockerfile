FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Debug -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/out .    