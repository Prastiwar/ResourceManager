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
                   .HasColumnType("INTEGER")
                   .HasConversion(to => Convert.ToInt32(to),
                                  from => from);

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

                // TODO: Configure requirements
                o.Ignore(x => x.Requirements);
            });

            // TODO: Configure requirements
            builder.Ignore(x => x.Requirements);

            //builder.Entity<Requirement>()
            //            .HasNoKey()
            //            .HasDiscriminator<string>("Type")
            //            .HasValue<DialogueRequirement>(typeof(DialogueRequirement).Name)
            //            .HasValue<ItemRequirement>(typeof(ItemRequirement).Name)
            //            .HasValue<MoneyRequirement>(typeof(MoneyRequirement).Name)
            //            .HasValue<QuestRequirement>(typeof(QuestRequirement).Name);

            //builder.Entity<ItemRequirement>()
            //            .Property(x => x.ItemId)
            //            .HasColumnType("INTEGER")
            //            .HasConversion(to => Convert.ToInt32(to),
            //                           from => from);

            //builder.Entity<DialogueRequirement>()
            //            .Property(x => x.DialogueId)
            //            .HasColumnType("INTEGER")
            //            .HasConversion(to => Convert.ToInt32(to),
            //                           from => from);

            //builder.Entity<QuestRequirement>()
            //            .Property(x => x.QuestId)
            //            .HasColumnType("INTEGER")
            //            .HasConversion(to => Convert.ToInt32(to),
            //                           from => from);
        }
    }
}