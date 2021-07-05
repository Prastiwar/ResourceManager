using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RpgDataEditor.Models;

namespace RpgDataEditor.DataSource
{
    public class QuestEntityConfiguration : IEntityTypeConfiguration<Quest>
    {
        public void Configure(EntityTypeBuilder<Quest> builder)
        {
            builder.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(x => x.Requirements)
                   .HasJsonConversion();

            builder.Property(x => x.Tasks)
                   .HasJsonConversion();

            builder.Property(x => x.CompletionTask)
                   .HasJsonConversion();
        }
    }
}