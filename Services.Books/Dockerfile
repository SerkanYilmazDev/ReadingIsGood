FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /api
COPY src/bin/Debug/net5.0 .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT Development
ENTRYPOINT dotnet Services.Books.dll