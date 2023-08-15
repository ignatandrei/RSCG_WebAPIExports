# RSCG_WebAPIExports

Add exports to file to WebAPI ( for the moment, just Excel / xlsx)

# Usage

Add reference to the package

```csharp
// Add services to the container.
//WebApiExportToFile.AddExport(builder.Services);
builder.Services.AddExport();

var app = builder.Build();
app.UseExport();

```

Add to any url : .xlsx ( e.g. for /WeatherForecast put /WeatherForecast.xlsx ) and the excel will be downloaded
