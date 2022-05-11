#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Budget.Web/Budget.Web.csproj", "Budget.Web/"]
COPY ["Budget.Repositories/Budget.Repositories.csproj", "Budget.Repositories/"]
COPY ["Budget.Core/Budget.Core.csproj", "Budget.Core/"]
COPY ["Budget.Common/Budget.Common.csproj", "Budget.Common/"]
COPY ["Budget.Persistance/Budget.Persistance.csproj", "Budget.Persistance/"]
COPY ["Budget.Infrastructure/Budget.Infrastructure.csproj", "Budget.Infrastructure/"]
RUN dotnet restore "Budget.Web/Budget.Web.csproj"
COPY . .
WORKDIR "/src/Budget.Web"
RUN dotnet build "Budget.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Budget.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Budget.Web.dll"]
