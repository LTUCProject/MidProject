using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MidProject.Data;
using MidProject.Models;
using MidProject.Repository.Interfaces;
using MidProject.Repository.Services;
using NuGet.Protocol.Core.Types;

namespace MidProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            string ConnectionStringVar = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<MidprojectDbContext>(optionsX => optionsX.UseSqlServer(ConnectionStringVar));

            builder.Services.AddIdentity<Account, IdentityRole>(options =>
            {
                // Identity options can be configured here
                options.User.RequireUniqueEmail = true; // Require unique email for users
            })
                .AddEntityFrameworkStores<MidprojectDbContext>()
                .AddDefaultTokenProviders(); // Register default token providers



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = JwtTokenService.ValidatToken(builder.Configuration);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder => builder.WithOrigins("http://localhost:5173") // Replace with React dev server
                                       .AllowAnyMethod()
                                       .AllowAnyHeader());
            });


            //policy 
            // Add authorization policies here
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("OwnerPolicy", policy =>
                    policy.RequireRole("Owner"));
                options.AddPolicy("ServicerPolicy", policy =>
                    policy.RequireRole("Servicer"));
                options.AddPolicy("ClientPolicy", policy =>
                    policy.RequireRole("Client"));
            });



            builder.Services.AddScoped<IAdmin, AdminServices>();
            builder.Services.AddScoped<IClient, ClientServices>();
            builder.Services.AddScoped<IOwner, OwnerServices>();
            builder.Services.AddScoped<IServicer, ServicerService>();
            builder.Services.AddScoped<IAccountx, IdentityAccountService>();
            builder.Services.AddScoped<JwtTokenService>();
            builder.Services.AddScoped<MailjetEmailService>();

            
            //============swagger============================

            // Swagger configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EV Management System API",
                    Version = "v1",
                    Description = "API for EV"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter user token below."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            //===================================

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();


            //====continue swagger==========================

            //call swagger services 
            app.UseSwagger
                (
                options =>
                {
                    options.RouteTemplate = "api/{documentName}/swagger.json";
                }
                );
            //call swagger UI
            app.UseSwaggerUI
                (
                options =>
                {
                    options.SwaggerEndpoint("/api/v1/swagger.json", "EV");
                    options.RoutePrefix = "";
                }
                );


            app.UseCors("AllowReactApp");

            //==================================
            app.MapControllers();

            //run
            app.Run();
        }
    }

}