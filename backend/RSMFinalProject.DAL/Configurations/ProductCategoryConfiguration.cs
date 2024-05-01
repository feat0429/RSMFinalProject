using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RSMFinalProject.Model;

namespace RSMFinalProject.DAL.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(e => e.ProductCategoryId).HasName("PK_ProductCategory_ProductCategoryID");

            builder.ToTable("ProductCategory", "Production", tb => tb.HasComment("High-level product categorization."));

            builder.HasIndex(e => e.Name, "AK_ProductCategory_Name").IsUnique();

            builder.HasIndex(e => e.Rowguid, "AK_ProductCategory_rowguid").IsUnique();

            builder.Property(e => e.ProductCategoryId)
                .HasComment("Primary key for ProductCategory records.")
                .HasColumnName("ProductCategoryID");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Category description.");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
        }
    }
}
