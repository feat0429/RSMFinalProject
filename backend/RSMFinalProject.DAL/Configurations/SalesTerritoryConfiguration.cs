namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;

    public class SalesTerritoryConfiguration : IEntityTypeConfiguration<SalesTerritory>
    {
        public void Configure(EntityTypeBuilder<SalesTerritory> builder)
        {
            builder.HasKey(e => e.TerritoryId).HasName("PK_SalesTerritory_TerritoryID");

            builder.ToTable("SalesTerritory", "Sales", tb => tb.HasComment("Sales territory lookup table."));

            builder.HasIndex(e => e.Name, "AK_SalesTerritory_Name").IsUnique();

            builder.HasIndex(e => e.Rowguid, "AK_SalesTerritory_rowguid").IsUnique();

            builder.Property(e => e.TerritoryId)
                .HasComment("Primary key for SalesTerritory records.")
                .HasColumnName("TerritoryID");
            builder.Property(e => e.CostLastYear)
                .HasComment("Business costs in the territory the previous year.")
                .HasColumnType("money");
            builder.Property(e => e.CostYtd)
                .HasComment("Business costs in the territory year to date.")
                .HasColumnType("money")
                .HasColumnName("CostYTD");
            builder.Property(e => e.CountryRegionCode)
                .HasMaxLength(3)
                .HasComment("ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode. ");
            builder.Property(e => e.Group)
                .HasMaxLength(50)
                .HasComment("Geographic area to which the sales territory belong.");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .HasComment("Sales territory description");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            builder.Property(e => e.SalesLastYear)
                .HasComment("Sales in the territory the previous year.")
                .HasColumnType("money");
            builder.Property(e => e.SalesYtd)
                .HasComment("Sales in the territory year to date.")
                .HasColumnType("money")
                .HasColumnName("SalesYTD");
        }
    }
}
