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
                    throw new NotSupportedException($"Type {type} for DbContext provider is not supported");
            }
        }

        public DbSet<Dialogue> Dialogues { get; set; }
        public DbSet<Npc> Npcs { get; set; }
        public DbSet<Quest> Quests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NpcJob>()
                        .HasDiscriminator<string>("Type")
                        .HasValue<TraderNpcJob>(typeof(TraderNpcJob).Name);

            modelBuilder.Entity<NpcJob>()
                        .Property<int>("OwnerId");

            modelBuilder.Entity<NpcJob>()
                        .HasKey("OwnerId");

            modelBuilder.Entity<TraderNpcJob>()
                        .Ignore(x => x.Items);

            //modelBuilder.Entity<TraderNpcJob>()
            //            .OwnsMany(x => x.Items, i => {
            //                i.WithOwner().HasForeignKey("OwnerId");
            //                i.Property<int>("Id");
            //                i.HasKey("Id");

            //                i.Property(x => x.ItemId)
            //                 .HasColumnType("INTEGER")
            //                 .HasConversion(to => Convert.ToInt32(to),
            //                                from => from);
            //            });


            modelBuilder.Entity<QuestTask>()
                        .HasDiscriminator<string>("Type")
                        .HasValue<DialogueQuestTask>(typeof(DialogueQuestTask).Name)
                        .HasValue<EntityInteractQuestTask>(typeof(EntityInteractQuestTask).Name)
                        .HasValue<ItemInteractQuestTask>(typeof(ItemInteractQuestTask).Name)
                        .HasValue<KillQuestTask>(typeof(KillQuestTask).Name)
                        .HasValue<ReachQuestTask>(typeof(ReachQuestTask).Name);

            modelBuilder.Entity<QuestTask>()
                        .Property<int>("OwnerId");

            modelBuilder.Entity<QuestTask>()
                        .HasKey("OwnerId");

            modelBuilder.Entity<DialogueQuestTask>()
                        .Property(x => x.DialogueId)
                        .HasColumnType("INTEGER")
                        .HasConversion(to => Convert.ToInt32(to),
                                       from => from);

            modelBuilder.Entity<EntityInteractQuestTask>()
                        .Property(x => x.EntityId)
                        .HasColumnType("INTEGER")
                        .HasConversion(to => Convert.ToInt32(to),
                                       from => from);

            modelBuilder.Entity<ItemInteractQuestTask>()
                        .Property(x => x.ItemId)
                        .HasColumnType("INTEGER")
                        .HasConversion(to => Convert.ToInt32(to),
                                       from => from);

            modelBuilder.Entity<KillQuestTask>()
                        .Property(x => x.TargetId)
                        .HasColumnType("INTEGER")
                        .HasConversion(to => Convert.ToInt32(to),
                                       from => from);

            modelBuilder.Entity<KillQuestTask>()
                        .Property(x => x.Amount)
                        .HasColumnType("INTEGER")
                        .HasConversion(to => Convert.ToInt32(to),
                                       from => from);

            modelBuilder.Entity<ReachQuestTask>()
                        .OwnsOne(x => x.Pos, p => {
                            p.Property(x => x.X)
                             .HasColumnType("INTEGER")
                             .HasColumnName("PosX");

                            p.Property(x => x.Y)
                             .HasColumnType("INTEGER")
                             .HasColumnName("PosY");

                            p.Property(x => x.Z)
                             .HasColumnType("INTEGER")
                             .HasColumnName("PosZ"); ;
                        });

            modelBuilder.ApplyConfiguration(new DialogueEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuestEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NpcEntityConfiguration());
        }
    }
}
