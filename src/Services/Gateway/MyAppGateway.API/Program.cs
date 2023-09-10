using MyAppGateway.API.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
.AddJsonFile(Path.Combine("Configurations", "Users", "configuration.json"))
.AddJsonFile(Path.Combine("Configurations", "Transactions", "configuration.json"))
.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// builder.Services.AddOcelot();
var app = builder.Build();
app.MapReverseProxy();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestCulture();

app.UseAuthorization();

app.MapControllers();
// app.UseOcelot().Wait();

app.Run();
