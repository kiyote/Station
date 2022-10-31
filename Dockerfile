FROM mcr.microsoft.com/dotnet/sdk:6.0 AS DotNetSDK

COPY . .
RUN dotnet restore

WORKDIR /src/Station/Server
RUN dotnet publish --no-restore --configuration Release --output /release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 As DotNetRuntime
WORKDIR /release
COPY --from=DotNetSDK /release ./
ENTRYPOINT ["executable", "Station.Server.exe"]

