FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["EliteApiBot/EliteApiBot.csproj", "EliteApiBot/"]
RUN dotnet restore "EliteApiBot/EliteApiBot.csproj"
COPY . .
WORKDIR "/src/EliteApiBot"
RUN dotnet build "EliteApiBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EliteApiBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EliteApiBot.dll"]