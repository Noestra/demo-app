using Demo.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Core.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.Property(x => x.Id)
            .HasColumnName("id");
        builder.Property(x => x.SiteId)
            .HasColumnName("site_id");
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(256);
        builder.Property(x => x.Unit)
            .HasColumnName("unit")
            .HasMaxLength(256);
        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(1024);
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasOne<Site>(x => x.Site)
            .WithMany(x => x.Tags)
            .OnDelete(DeleteBehavior.Restrict);
    }
}