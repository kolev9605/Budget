using Budget.Application;
using Budget.Infrastructure;
using Budget.Persistance;
using Budget.Persistance.Seeders;
using Budget.Web;
using Budget.Web.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastrucutreServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();

var app = builder.Build();

app.MapHealthChecks("/");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedAsync();

// Disable HTTPS redirection so the HTTP port works
// Temp solution
// app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyOrigin()
      .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapDefaultControllerRoute();

app.Run();
