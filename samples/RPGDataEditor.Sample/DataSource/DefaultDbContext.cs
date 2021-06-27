using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.DataSource
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext(string connectionString, IConfiguration configuration)
        {
            ConnectionString = connectionString;
            Configuration = configuration;
        }
        protected string ConnectionString { get; }
        protected IConfiguration Configuration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string type = Configuration["EngineName"].ToLower();
            switch (type)
            {
                case "sqlite":
                    optionsBuilder.UseSqlite(ConnectionString);
                    break;
                case "mssql":
                    optionsBuilder.UseSqlServer(ConnectionString);
                    break;
                default:
                    throw new System.NotSupportedException($"Type {type} for DbContext provider is not supported");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO: Improve model creation
            modelBuilder.Entity<Quest>();
            modelBuilder.Entity<Dialogue>();
            modelBuilder.Entity<Npc>();
        }
    }
}
