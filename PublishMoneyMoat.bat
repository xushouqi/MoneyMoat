
rem dotnet publish MoneyMoat\MoneyMoat.csproj -o ..\Publish -c Release

xcopy Publish\*.* MoneyMoat\obj\Docker\publish\ /s

pause