FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["efcore-postgres.csproj", "efcore-postgres/"]
RUN dotnet restore "efcore-postgres/efcore-postgres.csproj"
COPY . efcore-postgres/
WORKDIR "/src/efcore-postgres"
RUN dotnet build "efcore-postgres.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "efcore-postgres.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "efcore-postgres.dll"]