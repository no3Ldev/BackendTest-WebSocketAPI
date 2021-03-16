using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BackendTestWebSocket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            PrepareDatabase();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void PrepareDatabase()
        {
            try
            {
                var options = new DbContextOptions<DbContext>();
                using var context = new Models.AppDbContext(options);

                context.Database.EnsureCreated();
            }
            catch { throw; }
        }
    }
}
