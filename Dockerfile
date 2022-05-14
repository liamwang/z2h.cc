FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM base AS final
WORKDIR /app
COPY build .
ENTRYPOINT ["dotnet", "Server.dll"]