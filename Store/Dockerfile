FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY /bin/Release/net7.0/publish/ Store/

ENV ASPNETCORE_ENVIRONMENT Production
ENV Logging__Console__FormatterName=Simple

EXPOSE 5000
WORKDIR /Store
ENTRYPOINT ["dotnet", "Store.dll", "--urls=http://0.0.0.0:5000"]