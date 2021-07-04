using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RpgDataEditor.Models;
using System;

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

        public DbSet<Dialogue> Dialogues { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Quest> Quests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (string.IsNullOrEmpty(Configuration["EngineName"]))
            {
                throw new ArgumentNullException("EngineName", "Configuration must provide supported sql engine setting.");
            }
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
                    throw new NotSupportedException($"Type {type} for DbContext provider is not supported");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DialogueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuestEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NpcEntityConfiguration());
        }
    }
}
