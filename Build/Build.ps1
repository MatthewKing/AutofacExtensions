New-Item -Name ..\Packages -ItemType Directory -Force

# Build normal version...
..\Source\.nuget\NuGet.exe pack ..\Source\AutofacExtensions\AutofacExtensions.csproj -OutputDirectory ..\Packages\ -Build -Properties Configuration=Release

# Build embedded version...
$version = 'unknown'
Get-Content ..\Source\SharedAssemblyInfo.cs | ForEach-Object { if ($_ -match 'AssemblyInformationalVersion\("(.*)"\)') { $version = $matches[1] } }
New-Item -ItemType directory -Path .\Temp
Add-Content .\Temp\AutofacExtensions.cs ((Get-Content .\EmbeddedHeader.txt | ForEach-Object { '// ' + $_ }) -replace '{version}', $version)
Add-Content .\Temp\AutofacExtensions.cs ''
Add-Content .\Temp\AutofacExtensions.cs ((Get-Content ..\Source\AutofacExtensions\AutofacExtensions.cs) -replace 'public static class', 'internal static class')
Add-Content .\Temp\GlobalParameterModule.cs ((Get-Content .\EmbeddedHeader.txt | ForEach-Object { '// ' + $_ }) -replace '{version}', $version)
Add-Content .\Temp\GlobalParameterModule.cs ''
Add-Content .\Temp\GlobalParameterModule.cs ((Get-Content ..\Source\AutofacExtensions\GlobalParameterModule.cs) -replace 'public sealed class', 'internal sealed class')
Add-Content .\Temp\GlobalPropertyModule.cs ((Get-Content .\EmbeddedHeader.txt | ForEach-Object { '// ' + $_ }) -replace '{version}', $version)
Add-Content .\Temp\GlobalPropertyModule.cs ''
Add-Content .\Temp\GlobalPropertyModule.cs ((Get-Content ..\Source\AutofacExtensions\GlobalPropertyModule.cs) -replace 'public sealed class', 'internal sealed class')
Copy-Item ..\Source\AutofacExtensions\AutofacExtensions.Embedded.nuspec .\Temp\
..\Source\.nuget\NuGet.exe pack .\Temp\AutofacExtensions.Embedded.nuspec -OutputDirectory ..\Packages\
Remove-Item .\Temp\ -Recurse
