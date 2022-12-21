FROM public.ecr.aws/lambda/dotnet:6-arm64 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-amd64 as build

WORKDIR "/src/PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda"
COPY . .
RUN dotnet restore "PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.csproj"
RUN dotnet build "PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.csproj" \
            --configuration Release \
            --runtime linux-arm64 \
            --self-contained false \
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .
CMD ["PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda::PersonalInfrastructure.GamingPC.ScheduledShutdown.Lambda.Function::FunctionHandler"]