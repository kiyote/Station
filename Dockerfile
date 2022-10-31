FROM mcr.microsoft.com/dotnet/sdk:6.0 AS DotNetSDK

COPY *.sln .
COPY src/Station/Server.csproj ./server/
RUN dotnet restore

COPY src/Station/Server/. ./server/
WORKDIR /server
RUN dotnet publish --no-restore --configuration Release --output /release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 As DotNetRuntime
WORKDIR /release
COPY --from=DotNetSDK /release ./
ENTRYPOINT ["executable", "Station.Server.exe"]

