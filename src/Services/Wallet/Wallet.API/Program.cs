using Microsoft.EntityFrameworkCore;
using Wallet.API;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.LoggerService;

var builder = WebApplication.CreateBuilder(args);

// Host.CreateDefaultBuilder(args).ConfigureServices(services =>
// {
//     services.AddHostedService<BackgroundKafka>();
// });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WalletContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddHostedService<BackgroundKafka>();


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

