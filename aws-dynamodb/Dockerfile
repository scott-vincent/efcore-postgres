FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["aws-dynamodb.csproj", "aws-dynamodb/"]
RUN dotnet restore "aws-dynamodb/aws-dynamodb.csproj"
COPY . aws-dynamodb/
WORKDIR "/src/aws-dynamodb"
RUN dotnet build "aws-dynamodb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "aws-dynamodb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "aws-dynamodb.dll"]