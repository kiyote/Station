FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS DotNetStage
COPY src/Station /src
WORKDIR /src
RUN dotnet restore
WORKDIR /src/Server
RUN dotnet build --configuration Release --no-restore
RUN dotnet publish --no-build --no-restore --configuration Release --output release

WORKDIR /src/Server/release
COPY --from=DotNetStage . /
WORKDIR /

