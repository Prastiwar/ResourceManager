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

            // TODO: Map requirements
            builder.Ignore(x => x.Requirements);

            // TODO: Map tasks
            builder.Ignore(x => x.Tasks);

            // TODO: Map completion task
            builder.Ignore(x => x.CompletionTask);
        }
    }
}