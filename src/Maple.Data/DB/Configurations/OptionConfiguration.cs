using Maple.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple.Data
{
    public sealed class OptionConfiguration : IEntityTypeConfiguration<OptionModel>
    {
        public void Configure(EntityTypeBuilder<OptionModel> builder)
        {
            builder.Property(t => t.Key)
                   .IsRequired();

            builder.HasKey(t => t.Id);

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
        }
    }
}
