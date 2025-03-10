using System;
using System.Data;
using System.IO;
using Application.Behaviors;
using Domain.Abstractions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Web.Middleware;

using Application.Abstractions;
using Application.ChuckNorris.Queries.GetRandomJokeBySearch;
using Infrastructure.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Service;
using Application.Email.Command;

namespace Web;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        services.AddControllers()
            .AddApplicationPart(presentationAssembly);

        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

        services.AddMediatR(applicationAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddSwaggerGen(c =>
        {
            var presentationDocumentationFile = $"{presentationAssembly.GetName().Name}.xml";

            var presentationDocumentationFilePath =
                Path.Combine(AppContext.BaseDirectory, presentationDocumentationFile);

            c.IncludeXmlComments(presentationDocumentationFilePath);

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
        });

        services.AddDbContext<ApplicationDbContext>(builder =>
            builder.UseNpgsql(Configuration.GetConnectionString("Application")));

        services.AddScoped<IWebinarRepository, WebinarRepository>();

        services.AddScoped<IUnitOfWork>(
            factory => factory.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDbConnection>(
            factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());

        services.AddTransient<ExceptionHandlingMiddleware>();
        //////////////////////
        ///
        services.AddTransient<IChuckNorrisService, ChuckNorrisService>();
        services.AddHttpClient<IChuckNorrisService, ChuckNorrisService>(client =>
        {
            client.BaseAddress = new Uri("https://api.chucknorris.io/");
        });

        
        var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtOptions>();
        services.AddSingleton(jwtSettings);

        services.AddIdentity<AppUser, IdentityRole>(option =>
        {
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireUppercase = true;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequiredLength = 8;
        }).AddEntityFrameworkStores<ApplicationDbContext>();
       services.AddAuthentication(option => {
            option.DefaultAuthenticateScheme =
             option.DefaultChallengeScheme =
             option.DefaultForbidScheme =
             option.DefaultScheme =
             option.DefaultSignInScheme =
             option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
                )
            };
        });
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICacheService, CacheService>();


        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
        });
        var connectionString = Configuration.GetConnectionString("PostgreSqlConnection");

        // Cấu hình Hangfire sử dụng PostgreSQL theo cách mới
        services.AddHangfire(config =>
    config.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(connectionString);
    },
    new PostgreSqlStorageOptions
    {
        SchemaName = "hangfire", // Định nghĩa schema
      
    }));
        services.AddHangfireServer();
        services.Configure<EmailSettings>( Configuration.GetSection("EmailSettings"));
        services.AddScoped<IEmailService, EmailService>();
        services.AddMemoryCache();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication(); // Bật Authentication Middleware
        app.UseAuthorization();  // Bật Authorization Middleware
        app.UseHangfireDashboard("/hangfire");
       // app.UseHangfireServer();
       


        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}