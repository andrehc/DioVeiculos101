# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /Api

# Copia os arquivos do projeto e restaura depend�ncias
COPY . ./
RUN dotnet restore

# Builda a aplica��o
RUN dotnet publish -c Release -o /out

# Etapa de execu��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /out .

# Porta onde a aplica��o vai rodar
EXPOSE 80

# In�cio da aplica��o
ENTRYPOINT ["dotnet", "DioVeiculos.dll"]