using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using TestWebAPI;
using WebApiExportToFile;
//var responseContent = "[3,4,5]";

//dynamic valueData = JsonSerializer.Deserialize(responseContent, typeof(int[])) ;
//var data=new object[valueData.Length];
//for (int i = 0; i < valueData.Length; i++)
//{
//    data[i]= new { val = valueData[i] };
//}

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
//WebApiExportToFile.AddExport(builder.Services);
builder.Services.AddExport();

var app = builder.Build();
app.UseExport();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
