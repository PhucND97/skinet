using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Infrastructure.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using AutoMapper;
using API.Helpers;
using API.Middlewares;
using API.Errors;
using Microsoft.OpenApi.Models;
using API.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string sqlServerConnection = _configuration.GetConnectionString("SqlConnection");
            string sqliteConnection = _configuration.GetConnectionString("SqliteConnection");

            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddDbContext<StoreContext>(options =>
            {
                // options.UseSqlServer(sqlServerConnection);
                options.UseSqlite(sqliteConnection);
            });
            services.AddDbContext<AppIdentityDbContext>(options => 
            {
                options.UseSqlite(_configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddApplicationServices();
            services.AddIdentityExtension(_configuration);
            services.AddSwaggerDocumentation();
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
