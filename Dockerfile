FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BillLedger.slnx", "./"]
COPY ["BillLedger.API/BillLedger.API.csproj", "BillLedger.API/"]
COPY ["BillLedger.Application/BillLedger.Application.csproj", "BillLedger.Application/"]
COPY ["BillLedger.Domain/BillLedger.Domain.csproj", "BillLedger.Domain/"]
COPY ["BillLedger.Infrastructure/BillLedger.Infrastructure.csproj", "BillLedger.Infrastructure/"]

RUN dotnet restore "BillLedger.API/BillLedger.API.csproj"

COPY . .
WORKDIR "/src/BillLedger.API"
RUN dotnet publish "BillLedger.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/logs

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "BillLedger.API.dll"]