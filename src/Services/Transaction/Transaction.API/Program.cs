using Microsoft.EntityFrameworkCore;
using Transaction.Core.Contracts;
using Transaction.Infrastructure.Data;
using Transaction.Infrastructure.Host;
using Transaction.Infrastructure.UserClient;
using Transaction.Usecase.TransactionUsecase;
using Serilog;
using Serilog.Filters;
using Transaction.Infrastructure.Logger;
using Enjoyer.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.With(new ThreadEnricher())
                .WriteTo.Console()
                //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                //{
                //    AutoRegisterTemplate = true,
                //    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                //    NumberOfShards = 2,
                //    NumberOfReplicas = 1,
                //    ModifyConnectionSettings = x => x.BasicAuthentication(context.Configuration["ElasticConfiguration:Username"], context.Configuration["ElasticConfiguration:Password"]),
                //    MinimumLogEventLevel = Serilog.Events.LogEventLevel.Information,
                //    TypeName = null,
                //    BatchAction = ElasticOpType.Create
                //})
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration)
                .Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", v =>
                    "/".Equals(v, StringComparison.OrdinalIgnoreCase)));
            });

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.PropertyNamingPolicy = null;
           }); ;
builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITransactionUsecase, TransactionUsecase>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IHttpHost, HttpHost>();
builder.Services.AddTransient<IUserClient, UserClient>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();



app.Run();
