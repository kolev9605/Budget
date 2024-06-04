using Budget.Application;
using Budget.Infrastructure;
using Budget.Persistance;
using Budget.Api;
using Budget.Api.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .MinimumLevel.Information()
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
            builder.Environment.IsDevelopment() ?
            Serilog.Events.LogEventLevel.Information :
            Serilog.Events.LogEventLevel.Warning)

        .WriteTo.Console()
        .Enrich.FromLogContext();
});

builder.Services
    .AddPersistence(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

app.MapHealthChecks("/");

app.MapControllers();
// app.MapDefaultControllerRoute();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.Run();
