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

            builder.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(t => t.Id)
                    .HasColumnName("Id");

            builder.Property(t => t.Sequence)
                    .HasDefaultValue(0)
                    .HasColumnName("Sequence")
                    .IsRequired();

            builder.Property(t => t.CreatedBy)
                   .HasDefaultValue("SYSTEM");

            builder.Property(t => t.UpdatedBy)
                   .HasDefaultValue("SYSTEM");

            builder.Property(t => t.CreatedOn)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(t => t.UpdatedOn)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Ignore(c => c.IsDeleted);
        }
    }
}
