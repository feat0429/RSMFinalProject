

namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;

    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(e => e.AddressId).HasName("PK_Address_AddressID");

            builder.ToTable("Address", "Person", tb => tb.HasComment("Street address information for customers, employees, and vendors."));

            builder.HasIndex(e => e.Rowguid, "AK_Address_rowguid").IsUnique();

            builder.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.StateProvinceId, e.PostalCode }, "IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode").IsUnique();

            builder.HasIndex(e => e.StateProvinceId, "IX_Address_StateProvinceID");

            builder.Property(e => e.AddressId)
                .HasComment("Primary key for Address records.")
                .HasColumnName("AddressID");
            builder.Property(e => e.AddressLine1)
                .HasMaxLength(60)
                .HasComment("First street address line.");
            builder.Property(e => e.AddressLine2)
                .HasMaxLength(60)
                .HasComment("Second street address line.");
            builder.Property(e => e.City)
                .HasMaxLength(30)
                .HasComment("Name of the city.");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.PostalCode)
                .HasMaxLength(15)
                .HasComment("Postal code for the street address.");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            builder.Property(e => e.StateProvinceId)
                .HasComment("Unique identification number for the state or province. Foreign key to StateProvince table.")
                .HasColumnName("StateProvinceID");
        }
    }
}
