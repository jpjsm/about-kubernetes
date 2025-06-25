#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["rest-file-server.csproj", "."]
RUN dotnet restore "./rest-file-server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "rest-file-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rest-file-server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN mkdir -p /var/data
RUN chmod 777 /var/data
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rest-file-server.dll"]