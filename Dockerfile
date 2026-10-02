FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Example.Web/Example.Web.csproj Example.Web/
RUN dotnet restore Example.Web/Example.Web.csproj
COPY Example.Web/ Example.Web/
RUN dotnet publish Example.Web/Example.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "Example.Web.dll"]

