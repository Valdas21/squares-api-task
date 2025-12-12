using Microsoft.EntityFrameworkCore;                
using squares_api.Infrastructure.Data;
using squares_api.Infrastructure.Repositories;      
using squares_api.Application.Services;             
using squares_api.Domain.Interfaces;
using System.Text.Json.Serialization;
using Sentry.OpenTelemetry;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSentry(options =>
{
    options.Dsn = builder.Configuration["Sentry:Dsn"];
    options.SendDefaultPii = true;
    options.SampleRate = 1.0f;
    options.TracesSampleRate = 1.0;
    options.UseOpenTelemetry();
    options.Debug = true;
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PointsDb"));

builder.Services.AddScoped<IPointRepo, PointRepo>();
builder.Services.AddScoped<PointService>();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSentry();

var app = builder.Build();
SentrySdk.CaptureMessage("Hello Sentry");

try
{
    throw new Exception("Exception thrown for testing Sentry!");
}
catch
(Exception ex)
{
    SentrySdk.CaptureException(ex);
}



    using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
