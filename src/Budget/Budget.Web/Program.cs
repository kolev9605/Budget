using Budget.Core.Interfaces;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Infrastructure;
using Budget.Infrastructure.Services;
using Budget.Persistance;
using Budget.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IRecordRepository, RecordRepository>();

builder.Services.AddTransient<IRecordService, RecordService>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
