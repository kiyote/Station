FROM mcr.microsoft.com/dotnet/sdk:6.0 AS DotNetSDK
COPY src/Station /src
WORKDIR /src
RUN dotnet restore
WORKDIR /src/Server
RUN dotnet build --configuration Release --no-restore
RUN dotnet publish --no-build --no-restore --configuration Release --output release

FROM mcr.microsoft.com/dotnet/aspnet:6.0 As DotNetRuntime
WORKDIR /
COPY --from=DotNetSDK /src/Server/release /
WORKDIR /

