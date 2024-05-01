namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;
    public class ProductSubcategoryConfiguration : IEntityTypeConfiguration<ProductSubcategory>
    {
        public void Configure(EntityTypeBuilder<ProductSubcategory> builder)
        {
            builder.HasKey(e => e.ProductSubcategoryId).HasName("PK_ProductSubcategory_ProductSubcategoryID");

            builder.ToTable("ProductSubcategory", "Production", tb => tb.HasComment("Product subcategories. See ProductCategory table."));

            builder.HasIndex(e => e.Name, "AK_ProductSubcategory_Name").IsUnique();

            builder.HasIndex(e => e.Rowguid, "AK_ProductSubcategory_rowguid").IsUnique();

            builder.Property(e => e.ProductSubcategoryId)
                .HasComment("Primary key for ProductSubcategory records.")
                .HasColumnName("ProductSubcategoryID");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Subcategory description.");
            builder.Property(e => e.ProductCategoryId)
                .HasComment("Product category identification number. Foreign key to ProductCategory.ProductCategoryID.")
                .HasColumnName("ProductCategoryID");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");

            builder.HasOne(d => d.ProductCategory).WithMany(p => p.ProductSubcategories)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
