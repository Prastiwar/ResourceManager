using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RpgDataEditor.Models;

namespace RpgDataEditor.DataSource
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

        public DbSet<Dialogue> Dialogues { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Quest> Quests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DialogueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuestEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NpcEntityConfiguration());
        }
    }
}
