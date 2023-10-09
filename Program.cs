using ApiNotificaciones.Interfaces;
using ApiNotificaciones.Services;
using DotNetEnv;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;

namespace ApiNotificaciones
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<IKeycloakService, KeycloakService>();
            builder.Services.AddSingleton<IBancoRepository, BancoRepository>();
            builder.Services.AddSingleton<IRequestSender, RequestSender>();
            builder.Services.AddSingleton<IConsumer, Consumer>();
            builder.Services.AddControllers();
            builder.Services.AddSingleton(new SqlConnection(Env.GetString("CONNDB")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiNotification", Version = "v1" });
                c.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme()
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description =
                                "Enter your valid token in the text input below.\r\n\r\nExample: \"eyJhbGciOiJIUzI1NiIsInR5\"",
                        });
                c.AddSecurityRequirement(
                                new OpenApiSecurityRequirement
                                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            },
                            new string[] { }
                        },
                                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiNotification v1");
                    c.RoutePrefix = string.Empty;
                });
            

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}