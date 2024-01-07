FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 433

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ./src/EStore.Api/EStore.Api.csproj ./EStore.Api/
COPY ./src/EStore.Application/EStore.Application.csproj ./EStore.Application/
COPY ./src/EStore.Domain/EStore.Domain.csproj ./EStore.Domain/
COPY ./src/EStore.Infrastructure/EStore.Infrastructure.csproj ./EStore.Infrastructure/
COPY ./src/EStore.Contracts/EStore.Contracts.csproj ./EStore.Contracts/

# run restore over API project - this pulls restore over the dependent projects as well
RUN dotnet restore ./EStore.Api/EStore.Api.csproj

# Copy everything else
COPY ./src .

# run build over the API project
WORKDIR /src/EStore.Api
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "EStore.Api.dll" ]
