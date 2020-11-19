#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DeliveryMomentCosmosChangeStream/DeliveryMomentCosmosChangeStream.csproj", "DeliveryMomentCosmosChangeStream/"]
COPY ["DeliveryMomentCosmosRepository/DeliveryMomentCosmosRepository.csproj", "DeliveryMomentCosmosRepository/"]
COPY ["ConfluentKafkaUtility/ConfluentKafkaUtility.csproj", "ConfluentKafkaUtility/"]
RUN dotnet restore "DeliveryMomentCosmosChangeStream/DeliveryMomentCosmosChangeStream.csproj"
COPY . .
WORKDIR "/src/DeliveryMomentCosmosChangeStream"
RUN dotnet build "DeliveryMomentCosmosChangeStream.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeliveryMomentCosmosChangeStream.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliveryMomentCosmosChangeStream.dll"]