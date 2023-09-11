using System;
using System.IO;
using System.Text;
using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Core.Services;
using Auth.Infrastructure;
using Auth.Infrastructure.Data;
using Auth.Usecase.UserUsecase;
using Enjoyer.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;

namespace Auth.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            string nlogPath = "/nlog.config";
            if (env.IsDevelopment())
            {
                nlogPath = "/nlog.dev.config";
            }
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), nlogPath));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;

            }).AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.PropertyNamingPolicy = null;
           });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["ValidIssuer"],
                    ValidAudience = Configuration["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                };
            });

            // var connection = $"Server={{DB_HOST}};User Id={{DB_USER}};Password={{DB_PASSWORD}};Database={{DB_NAME}};Integrated Security=false;";
            services.AddDbContext<ApplicationUserContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Connection")));
            // services.AddDbContext<ApplicationUserContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationUserContext>().AddDefaultTokenProviders();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"{Configuration["SwaggerTitle"]} {Configuration["SwaggerVersion"]}", new OpenApiInfo { Title = Configuration["SwaggerTitle"], Version = Configuration["SwaggerVersion"] });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<IHashing, HashingService>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IUserUsecase, UserUsecase>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            //app.UseAllElasticApm(Configuration);

            app.UseStaticFiles();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseRouting();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
