FROM microsoft/aspnetcore:2.0
ARG source
WORKDIR /app
EXPOSE 15046
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MoneyMoat.dll"]
