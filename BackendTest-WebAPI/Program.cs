using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BackendTestWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            PrepareDatabase();
            CreateHostBuilder(args).Build().Run();
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

                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            catch { throw; }
        }
    }
}
