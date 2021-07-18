# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . ./
RUN dotnet restore
COPY . ./

# testing
# FROM build AS testing
# WORKDIR /src/Colors.API
# RUN dotnet build
# WORKDIR /src/Colors.UnitTests
# RUN dotnet test

# Build
WORKDIR /src
RUN dotnet build

# publish
FROM build AS publish
WORKDIR /src
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "ContactBook.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContactBook.dll
