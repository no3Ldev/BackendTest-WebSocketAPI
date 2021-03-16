using BackendTestWebSocket;
using BackendTestWebSocket.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BackendTestUnitTest
{
    public class _BaseClass
    {
        private static WebApplicationFactory<Startup> _factory;

        protected const string CONTENT_TYPE_TEXT = "text/plain; charset=utf-8";
        protected const string CONTENT_TYPE_JSON = "application/json; charset=utf-8";

        protected static JsonSerializerOptions _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        protected static AppDbContext _context;
        protected static IConfiguration _config;

        protected static void BaseInitialize()
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder.UseSetting("https_port", "5001")
                .UseEnvironment("Testing"));

            _config = _factory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            _context = _factory.Services.GetService(typeof(AppDbContext)) as AppDbContext;
        }

        protected static void GetContext()
        {
        }

        protected static void BaseDispose()
        {
            _factory.Dispose();
        }
    }
}
