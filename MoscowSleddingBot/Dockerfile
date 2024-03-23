FROM mcr.microsoft.com/dotnet/runtime:6.0-focal as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY ["MoscowSleddingBot/MoscowSleddingBot.csproj", "MoscowSleddingBot/"]
RUN dotnet restore "MoscowSleddingBot/MoscowSleddingBot.csproj"
COPY . .
WORKDIR "/source/MoscowSleddingBot"
RUN dotnet build "MoscowSleddingBot.csproj" -c Release -o /app/build

WORKDIR "/source/MoscowSleddingBot"
FROM build as publish
RUN dotnet publish "MoscowSleddingBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
COPY MoscowSleddingBot/LoadedData /app/LoadedData/
COPY MoscowSleddingBot/var /app/var/
COPY MoscowSleddingBot/Assets/ice-hills.csv /app/Assets/ice-hills.csv
ENTRYPOINT ["dotnet", "MoscowSleddingBot.dll"]