FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY NetCoreApp/NetCoreApp.csproj NetCoreApp/
COPY NetCore.EntityFrameworkCore/NetCore.EntityFrameworkCore.csproj NetCore.EntityFrameworkCore/
COPY NetCore.Core/NetCore.Core.csproj NetCore.Core/
COPY NetCore.Repository/NetCore.Repository.csproj NetCore.Repository/
COPY NetCore.DTO/NetCore.DTO.csproj NetCore.DTO/
COPY NetCore.Domain/NetCore.Domain.csproj NetCore.Domain/
COPY NetCore.Services/NetCore.Services.csproj NetCore.Services/
RUN dotnet restore NetCoreApp/NetCoreApp.csproj
COPY . .
WORKDIR /src/NetCoreApp
RUN dotnet build NetCoreApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NetCoreApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCoreApp.dll"]
