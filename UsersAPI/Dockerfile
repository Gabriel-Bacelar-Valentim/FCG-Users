FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FCG.Users/UsersAPI/UsersAPI.csproj", "FCG.Users/UsersAPI/"]
COPY ["FCG.Users/FCG.Application/FCG.Application.csproj", "FCG.Users/FCG.Application/"]

COPY ["FCG.Orchestration/FCG.Contracts/FCG.Contracts.csproj", "FCG.Orchestration/FCG.Contracts/"]
COPY ["FCG.Orchestration/FCG.Domain/FCG.Domain.csproj", "FCG.Orchestration/FCG.Domain/"]
COPY ["FCG.Orchestration/FCG.Infrastructure/FCG.Infrastructure.csproj", "FCG.Orchestration/FCG.Infrastructure/"]

RUN dotnet restore "FCG.Users/UsersAPI/UsersAPI.csproj"

COPY . .

RUN dotnet publish "FCG.Users/UsersAPI/UsersAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UsersAPI.dll"]