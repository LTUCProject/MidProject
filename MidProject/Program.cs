

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MidProject.Data;

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

            //============swagger============================

            builder.Services.AddSwaggerGen
                (
                option =>
                {
                    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "EV api doc",
                        Version = "v1",
                        Description = "Api for EV"
                        
                    });
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Please enter user token below."
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                }
                );

            //===================================

            var app = builder.Build();


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
                    options.SwaggerEndpoint("/api/v1/swagger.json", "Tunify Api");
                    options.RoutePrefix = "";
                }
                );

            //==================================
            app.MapControllers();
            app.MapGet("/", () => "Hello World!");
            //run
            app.Run();
        }
    }

}