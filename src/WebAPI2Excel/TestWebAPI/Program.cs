using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using TestWebAPI;
using WebApiExportToFile;

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
