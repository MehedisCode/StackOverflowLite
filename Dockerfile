FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY *.slnx ./
COPY src/StackOverflowLite.Api/*.csproj src/StackOverflowLite.Api/
COPY src/StackOverflowLite.Application/*.csproj src/StackOverflowLite.Application/
COPY src/StackOverflowLite.Domain/*.csproj src/StackOverflowLite.Domain/
COPY src/StackOverflowLite.Infrastructure/*.csproj src/StackOverflowLite.Infrastructure/

RUN dotnet restore StackOverflowLite.slnx

# Copy the rest of the source code
COPY src/ src/

# Publish
WORKDIR /src/src/StackOverflowLite.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Standard port for modern ASP.NET Core
EXPOSE 8080

ENTRYPOINT ["dotnet", "StackOverflowLite.Api.dll"]
