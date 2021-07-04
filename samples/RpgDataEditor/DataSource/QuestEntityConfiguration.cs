using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgDataEditor.Models;
using System;

namespace RpgDataEditor.DataSource
{
    public class QuestEntityConfiguration : IEntityTypeConfiguration<Quest>
    {
        public void Configure(EntityTypeBuilder<Quest> builder)
        {
            builder.Property(x => x.Id)
                   .HasColumnType("INTEGER")
                   .HasConversion(to => Convert.ToInt32(to),
                                  from => from);

            builder.Property(x => x.Requirements)
                   .HasJsonConversion();

            builder.Property(x => x.Tasks)
                   .HasJsonConversion();

            builder.Property(x => x.CompletionTask)
                   .HasJsonConversion();
        }
    }
}