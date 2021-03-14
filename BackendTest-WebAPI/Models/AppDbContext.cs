using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BackendTestWebAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Verification> Verifications { get; set; }

        private readonly IConfiguration _config;

        public AppDbContext(DbContextOptions options) : base(options)
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_config.GetConnectionString("Datasource"));
    }
}
