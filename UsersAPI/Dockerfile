FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FCG.Users/UsersAPI/UsersAPI.csproj", "FCG.Users/UsersAPI/"]
COPY ["FCG.Contracts/FCG.Contracts.csproj", "FCG.Contracts/"]
COPY ["FCG.Domain/FCG.Domain.csproj", "FCG.Domain/"]
COPY ["FCG.Infrastructure/FCG.Infrastructure.csproj", "FCG.Infrastructure/"]
COPY ["FCG.Users/FCG.Application/FCG.Application.csproj", "FCG.Users/FCG.Application/"]

RUN dotnet restore "FCG.Users/UsersAPI/UsersAPI.csproj"

COPY . .

RUN dotnet publish "FCG.Users/UsersAPI/UsersAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UsersAPI.dll"]