using Microsoft.EntityFrameworkCore;
using RPGDataEditor.Sample.Models;

namespace RPGDataEditor.Sample.DataSource
{
    public class DefaultDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (true)
            {
                // TODO: configure efcore provider
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Quest>();
            modelBuilder.Entity<Dialogue>();
            modelBuilder.Entity<Npc>();
        }
    }
}
