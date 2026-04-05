# Estágio 1: Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto e restaura as dependências
COPY ["Reporta_Colombo_Back.csproj", "./"]
RUN dotnet restore "Reporta_Colombo_Back.csproj"

# Copia o restante dos arquivos e compila
COPY . .
RUN dotnet publish "Reporta_Colombo_Back.csproj" -c Release -o /app/publish

# Estágio 2: Execução (Imagem final mais leve)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# O Render usa a porta 10000 por padrão, vamos configurar o .NET para ouvi-la
ENV ASPNETCORE_URLS=http://*:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "SeuProjeto.dll"]