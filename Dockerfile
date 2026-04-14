FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MOBILE.SIGE.csproj", "./"]
RUN dotnet restore "MOBILE.SIGE.csproj"

COPY . .
RUN dotnet publish "MOBILE.SIGE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet MOBILE.SIGE.dll"]
