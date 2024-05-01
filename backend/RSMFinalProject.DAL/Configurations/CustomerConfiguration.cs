namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;

    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.CustomerId).HasName("PK_Customer_CustomerID");

            builder.ToTable("Customer", "Sales", tb => tb.HasComment("Current customer information. Also see the Person and Store tables."));

            builder.HasIndex(e => e.AccountNumber, "AK_Customer_AccountNumber").IsUnique();

            builder.HasIndex(e => e.Rowguid, "AK_Customer_rowguid").IsUnique();

            builder.HasIndex(e => e.TerritoryId, "IX_Customer_TerritoryID");

            builder.Property(e => e.CustomerId)
                .HasComment("Primary key.")
                .HasColumnName("CustomerID");
            builder.Property(e => e.AccountNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComputedColumnSql("(isnull('AW'+[dbo].[ufnLeadingZeros]([CustomerID]),''))", false)
                .HasComment("Unique number identifying the customer assigned by the accounting system.");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.PersonId)
                .HasComment("Foreign key to Person.BusinessEntityID")
                .HasColumnName("PersonID");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            builder.Property(e => e.StoreId)
                .HasComment("Foreign key to Store.BusinessEntityID")
                .HasColumnName("StoreID");
            builder.Property(e => e.TerritoryId)
                .HasComment("ID of the territory in which the customer is located. Foreign key to SalesTerritory.SalesTerritoryID.")
                .HasColumnName("TerritoryID");

            builder.HasOne(d => d.Person).WithMany(p => p.Customers).HasForeignKey(d => d.PersonId);
        }
    }
}
