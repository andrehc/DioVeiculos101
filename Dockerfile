# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /Api

# Copia os arquivos do projeto e restaura dependências
COPY . ./
RUN dotnet restore

# Builda a aplicação
RUN dotnet publish -c Release -o /out

# Etapa de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /out .

# Porta onde a aplicação vai rodar
EXPOSE 80

# Início da aplicação
ENTRYPOINT ["dotnet", "DioVeiculos.dll"]