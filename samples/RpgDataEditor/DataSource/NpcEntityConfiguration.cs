using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using RpgDataEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgDataEditor.DataSource
{
    public class NpcEntityConfiguration : IEntityTypeConfiguration<Npc>
    {
        public void Configure(EntityTypeBuilder<Npc> builder)
        {
            builder.Property(x => x.Id)
                   .HasColumnType("INTEGER")
                   .HasConversion(to => Convert.ToInt32(to),
                                  from => from);

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
                 .HasConversion(v => JsonConvert.SerializeObject(v),
                                v => JsonConvert.DeserializeObject<List<int>>(v),
                                new ValueComparer<List<int>>(
                                    (c1, c2) => c1.SequenceEqual(c2),
                                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                                    c => c.ToList()));
            });

            builder.HasOne(x => x.Job)
                   .WithMany()
                   .HasForeignKey("OwnerId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}