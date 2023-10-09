FROM dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443
EXPOSE 7071
EXPOSE 5108



FROM dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .

RUN dotnet nuget add source 'http://172.22.227.36:8081/repository/nuget.org-proxy/' -n nuget-nexux.org
RUN dotnet restore "ApiNotificaciones.csproj"

RUN dotnet build "ApiNotificaciones.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiNotificaciones.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiNotificaciones.dll"]