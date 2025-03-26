FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore "./Budget.Api/Budget.Api.csproj"
RUN dotnet publish "./Budget.Api/Budget.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Budget.Api.dll"]
