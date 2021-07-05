using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgDataEditor.Models;

namespace RpgDataEditor.DataSource
{
    public class NpcEntityConfiguration : IEntityTypeConfiguration<Npc>
    {
        public void Configure(EntityTypeBuilder<Npc> builder)
        {
            builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                   .HasColumnType("TEXT");

            builder.OwnsOne(x => x.Position, p => {
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

            builder.OwnsMany(x => x.Attributes, a => {
                a.WithOwner().HasForeignKey("OwnerId");
                a.Property<int>("Id");
                a.HasKey("Id");
            });

            builder.OwnsOne(x => x.TalkData, t => {
                t.Property(x => x.InitationDialogues)
                 .HasJsonConversion();
            });

            builder.Property(x => x.Job)
                   .HasJsonConversion();
        }
    }
}