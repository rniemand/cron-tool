FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/DockerCronTool/DockerCronTool.csproj", "DockerCronTool/"]
COPY ["src/CronTools.Common/CronTools.Common.csproj", "CronTools.Common/"]

RUN dotnet restore "DockerCronTool/DockerCronTool.csproj"

COPY ["src/DockerCronTool/", "DockerCronTool/"]
COPY ["src/CronTools.Common/", "CronTools.Common/"]

WORKDIR "/src/DockerCronTool"
RUN dotnet build "DockerCronTool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerCronTool.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerCronTool.dll"]
