# Estágio 1: Build (Usando o SDK 10.0)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o arquivo .csproj específico
COPY ["Reporta_Colombo_Back/Reporta_Colombo_Back.csproj", "Reporta_Colombo_Back/"]
RUN dotnet restore "Reporta_Colombo_Back/Reporta_Colombo_Back.csproj"

# Copia o resto e compila
COPY . .
WORKDIR "/src/Reporta_Colombo_Back"
RUN dotnet build "Reporta_Colombo_Back.csproj" -c Release -o /app/build

# Estágio 2: Publish
FROM build AS publish
RUN dotnet publish "Reporta_Colombo_Back.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Runtime (Usando o ASP.NET 10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configuração de porta para o Render
ENV ASPNETCORE_URLS=http://*:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Reporta_Colombo_Back.dll"]
