# RSCG_WebAPIExports

Add exports to file to WebAPI ( for the moment, just Excel / xlsx)

# Usage

Add reference to the package in the .csproj
```xml
<PackageReference Include="RSCG_WebAPIExports" Version="2023.8.16.1219" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<!--
<PackageReference Include="RSCG_WebAPIExports" Version="2023.8.16.1219" OutputItemType="Analyzer" ReferenceOutputAssembly="true"  />
<PackageReference Include="ArrayToExcel" Version="2.2.2" />
-->
```

Then in the WebAPI add
```csharp
using TestWebAPI;
//code
// Add services to the container.
//WebApiExportToFile.AddExport(builder.Services);
builder.Services.AddExport();
var app = builder.Build();
app.UseExport();

```

Add to any url : .xlsx ( e.g. for /WeatherForecast put /WeatherForecast.xlsx ) and the excel will be 

downloaded
