using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgDataEditor.Models;
using System;

namespace RpgDataEditor.DataSource
{
    public class DialogueEntityConfiguration : IEntityTypeConfiguration<Dialogue>
    {
        public void Configure(EntityTypeBuilder<Dialogue> builder)
        {
            builder.ToTable("Dialogues");

            builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(x => x.StartQuestId)
                   .HasColumnType("INTEGER")
                   .HasConversion(to => Convert.ToInt32(to),
                                  from => from);

            builder.OwnsMany(x => x.Options, o => {
                o.WithOwner().HasForeignKey("OwnerId");
                o.Property<int>("Id");
                o.HasKey("Id");

                o.Property(x => x.NextDialogId)
                 .HasColumnType("INTEGER")
                 .HasConversion(to => Convert.ToInt32(to),
                                from => from);

                o.Property(x => x.Requirements)
                 .HasJsonConversion();
            });

            builder.Property(x => x.Requirements)
                   .HasJsonConversion();
        }
    }
}