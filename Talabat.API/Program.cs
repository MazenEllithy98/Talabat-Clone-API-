using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Helpers;
using Talabat.API.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.API
{
    public class Program
    {
        //Entry Point
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(); // API Services

            builder.Services.AddSwaggerServices();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //ApplicationServicesExtension.AddApplicationServices(builder.Services);
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            builder.Services.AddApplicationServices();

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("MyPolicy" , options =>
            //    {
            //        options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            //    })
            //});

            builder.Services.AddIdentityServices(builder.Configuration);


            #endregion

                        var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = services.GetRequiredService<StoreContext>(); //Ask Explicitly
                await dbContext.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(dbContext);
                
                var identityContext = services.GetRequiredService<ApplicationIdentityDbContext>();
                await identityContext.Database.MigrateAsync();

                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                await ApplicationIdentityDbContextSeed.SeedUsersAsync(userManager);
            
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Happened while applying Migration");
            }

            #region Configure Kestrel

            app.UseMiddleware<ExceptionMiddleware>();
            
            
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();

            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();
            
            app.MapControllers();
            #endregion

            app.Run();
        }
    }
}