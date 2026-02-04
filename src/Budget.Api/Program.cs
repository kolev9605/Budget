using Budget.Api;
using Budget.Api.Helpers;
using Budget.Api.Infrastructure;
using Budget.Api.Infrastructure.Persistence;
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
    .AddPersistence(builder.Configuration.GetConnectionString("Budget"))
    .AddInfrastructure(builder.Configuration)
    .AddPresentation()
    // .AddAI(builder.Configuration)
;

var app = builder.Build();

app.MapHealthChecks("/");

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable HTTPS redirection so the HTTP port works
app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .AllowAnyOrigin()
      .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();

public partial class Program { }
