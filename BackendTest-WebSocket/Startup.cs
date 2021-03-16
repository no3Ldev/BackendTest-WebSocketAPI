using BackendTestWebSocket.Models;
using BackendTestWebSocket.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackendTestWebSocket
{
    public class Startup
    {
        private IConfiguration _config { get; }
        private IWebHostEnvironment _environment { get; }

        public Startup(IConfiguration config, IWebHostEnvironment environment)
        {
            _config = config;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.EnvironmentName == "Testing")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(_config.GetConnectionString("Datasource")), ServiceLifetime.Singleton);
            }
            else
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(_config.GetConnectionString("Datasource")));
            }

            services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            var serviceHandler = serviceProvider.GetService<WebSocketRequestHandler>();

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", serviceHandler);
        }
    }
}
