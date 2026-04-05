# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia o arquivo .csproj DE DENTRO da subpasta para a raiz do WORKDIR no Docker
COPY ["Reporta_Colombo_Back/Reporta_Colombo_Back.csproj", "Reporta_Colombo_Back/"]
RUN dotnet restore "Reporta_Colombo_Back/Reporta_Colombo_Back.csproj"

# 2. Copia todo o conteúdo da pasta do projeto
COPY . .
WORKDIR "/src/Reporta_Colombo_Back"
RUN dotnet build "Reporta_Colombo_Back.csproj" -c Release -o /app/build

# Estágio 2: Publish
FROM build AS publish
RUN dotnet publish "Reporta_Colombo_Back.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://*:10000
EXPOSE 10000

# Certifique-se de que o nome da DLL seja exatamente o que o VS gera
ENTRYPOINT ["dotnet", "Reporta_Colombo_Back.dll"]
