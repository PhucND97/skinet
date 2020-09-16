using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using AutoMapper;
using API.Helpers;
using API.Middlewares;
using API.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>(options =>
            {
                // options.UseSqlServer(sqlServerConnection);
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<AppIdentityDbContext>(options => 
            {
                options.UseSqlite(_configuration.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>(options =>
            {
                // options.UseSqlServer(sqlServerConnection);
                options.UseMySql(_configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<AppIdentityDbContext>(options => 
            {
                options.UseMySql(_configuration.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(MappingProfile));
           
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
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/content"
            });

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
