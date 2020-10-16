FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY ./publish/ /app 
EXPOSE 44321
ENTRYPOINT ["dotnet","WebApi.dll"]