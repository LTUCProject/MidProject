

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MidProject.Data;
using MidProject.Repositories.Interfaces;
using MidProject.Repositories.Services;
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
            builder.Services.AddScoped<IBooking, BookingServices>();
            builder.Services.AddScoped<ICharger, ChargerServices>();
            builder.Services.AddScoped<IChargingStation, ChargingStationServices>();
            builder.Services.AddScoped<IChargingStation, ChargingStationServices>();
            builder.Services.AddScoped<IComment, CommentServices>();
            builder.Services.AddScoped<IFavorite, FavoriteServices>();
            builder.Services.AddScoped<IFeedback, FeedbackServices>();
            builder.Services.AddScoped<ILocation, LocationServices>();
            builder.Services.AddScoped<IMaintenanceLog, MaintenanceLogServices>();
            builder.Services.AddScoped<INotification, NotificationServices>();
            builder.Services.AddScoped<IPaymentTransaction, PaymentTransactionServices>();
            builder.Services.AddScoped<IPost, PostServices>();
            builder.Services.AddScoped<IServiceInfo, ServiceInfoServices>();
            builder.Services.AddScoped<IServiceRequest, ServiceRequestServices>();
            builder.Services.AddScoped<ISessionx, SessionServices>();
            builder.Services.AddScoped<ISubscriptionPlan, SubscriptionPlanServices>();
            builder.Services.AddScoped<IUser, UserServices>();
            builder.Services.AddScoped<IUserSubscription, UserSubscriptionServices>();
            builder.Services.AddScoped<IVehicle, VehicleServices>();



















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