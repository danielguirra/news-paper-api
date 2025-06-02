# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/out ./

COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
RUN sed -i 's/\r$//' entrypoint.sh

EXPOSE 80
ENTRYPOINT ["./entrypoint.sh"]
