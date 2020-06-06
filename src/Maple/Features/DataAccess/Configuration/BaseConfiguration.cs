using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple
{
    public abstract class BaseConfiguration<T, TKey> : IEntityTypeConfiguration<T>
        where T : Entity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                    .HasColumnName("Id");

            builder.Property(t => t.Sequence)
                    .HasDefaultValue(0)
                    .HasColumnName("Sequence")
                    .IsRequired();

            builder.Property(t => t.Name)
                    .HasDefaultValue("unknown")
                    .HasColumnName("Name")
                    .HasMaxLength(128);

            builder.Property(t => t.CreatedBy)
                    .HasDefaultValue("Maple")
                    .HasColumnName("CreatedBy");

            builder.Property(t => t.UpdatedBy)
                    .HasDefaultValue("Maple")
                    .HasColumnName("UpdatedBy");

            builder.Property(t => t.CreatedOn)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("CreatedOn");

            builder.Property(t => t.UpdatedOn)
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("UpdatedOn");
        }
    }
}
