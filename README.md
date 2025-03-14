[![RSCG_WebAPIExports](https://img.shields.io/nuget/v/RSCG_WebAPIExports?label=RSCG_WebAPIExports)](https://www.nuget.org/packages/RSCG_WebAPIExports/)

# RSCG_WebAPIExports

Add exports to file to WebAPI ( for the moment, just Excel / xlsx)



## How to use in WebAPI project


Add reference to the package in the .csproj
```xml
<PackageReference Include="RSCG_WebAPIExports" Version="2025.8.310.2000" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<!--
<PackageReference Include="RSCG_WebAPIExports" Version="2025.8.310.2000" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<PackageReference Include="ArrayToExcel" Version="2.2.2" />
-->
```
or
```xml
<PackageReference Include="RSCG_WebAPIExports" Version="2025.8.310.2000" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<!--
<PackageReference Include="RSCG_WebAPIExports" Version="2025.8.310.2000" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<PackageReference Include="ArrayToExcel" Version="2.2.2" />
-->
```


Then in the WebAPI add
```csharp
using WebApiExportToFile;
//code
// Add services to the container.
//WebApiExportToFile.AddExport(builder.Services);
builder.Services.AddExport();
var app = builder.Build();
app.UseExport();

```

Add to any url : .xlsx or .csv ( e.g. 

for /WeatherForecast put /WeatherForecast.xlsx 

for /WeatherForecast/GetPerson?id=23 put /WeatherForecast/GetPerson.xlsx?id=23

) 

and the excel will be downloaded

## Examples

JSON value type array: 
https://tiltwebapp.azurewebsites.net/api/PublicTILTs/PublicTiltsURL


Excel:
https://tiltwebapp.azurewebsites.net/api/PublicTILTs/PublicTiltsURL.xlsx


JSON object array:
https://tiltwebapp.azurewebsites.net/api/PublicTILTs/LatestTILTs/ignatandrei/10

Excel:
https://tiltwebapp.azurewebsites.net/api/PublicTILTs/LatestTILTs/ignatandrei/10.xlsx