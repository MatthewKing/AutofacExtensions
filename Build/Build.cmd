@echo off
mkdir Packages
..\Source\.nuget\NuGet.exe pack ..\Source\AutofacExtensions\AutofacExtensions.csproj -OutputDirectory Packages\ -Build -Properties Configuration=Release
..\Source\.nuget\NuGet.exe pack ..\Source\AutofacExtensions\AutofacExtensions.Embedded.nuspec -OutputDirectory Packages\