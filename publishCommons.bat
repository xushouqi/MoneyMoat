
dotnet publish CommonLibs\CommonLibs.csproj -o ..\..\PublishCore -c Release
dotnet publish CommonNetwork\CommonNetwork.csproj -o ..\..\PublishCore -c Release
dotnet publish CommonServices\CommonServices.csproj -o ..\..\PublishCore -c Release

pause
