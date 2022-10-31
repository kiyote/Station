FROM mcr.microsoft.com/dotnet/sdk:6.0 AS DotNetSDK

COPY ./src/Station .
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore
RUN dotnet publish --no-build --no-restore --configuration Release --output /release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 As DotNetRuntime
WORKDIR /release
COPY --from=DotNetSDK /release /
ENTRYPOINT ["executable", "Station.Server.exe"]

