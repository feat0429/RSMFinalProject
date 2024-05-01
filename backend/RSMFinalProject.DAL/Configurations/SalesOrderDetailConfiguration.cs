namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;
    public class SalesOrderDetailConfiguration : IEntityTypeConfiguration<SalesOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SalesOrderDetail> builder)
        {
            builder.HasKey(e => new { e.SalesOrderId, e.SalesOrderDetailId }).HasName("PK_SalesOrderDetail_SalesOrderID_SalesOrderDetailID");

            builder.ToTable("SalesOrderDetail", "Sales", tb =>
            {
                tb.HasComment("Individual products associated with a specific sales order. See SalesOrderHeader.");
                tb.HasTrigger("iduSalesOrderDetail");
            });

            builder.HasIndex(e => e.Rowguid, "AK_SalesOrderDetail_rowguid").IsUnique();

            builder.HasIndex(e => e.ProductId, "IX_SalesOrderDetail_ProductID");

            builder.Property(e => e.SalesOrderId)
                .HasComment("Primary key. Foreign key to SalesOrderHeader.SalesOrderID.")
                .HasColumnName("SalesOrderID");
            builder.Property(e => e.SalesOrderDetailId)
                .ValueGeneratedOnAdd()
                .HasComment("Primary key. One incremental unique number per product sold.")
                .HasColumnName("SalesOrderDetailID");
            builder.Property(e => e.CarrierTrackingNumber)
                .HasMaxLength(25)
                .HasComment("Shipment tracking number supplied by the shipper.");
            builder.Property(e => e.LineTotal)
                .HasComputedColumnSql("(isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0)))", false)
                .HasComment("Per product subtotal. Computed as UnitPrice * (1 - UnitPriceDiscount) * OrderQty.")
                .HasColumnType("numeric(38, 6)");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.OrderQty).HasComment("Quantity ordered per product.");
            builder.Property(e => e.ProductId)
                .HasComment("Product sold to customer. Foreign key to Product.ProductID.")
                .HasColumnName("ProductID");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            builder.Property(e => e.SpecialOfferId)
                .HasComment("Promotional code. Foreign key to SpecialOffer.SpecialOfferID.")
                .HasColumnName("SpecialOfferID");
            builder.Property(e => e.UnitPrice)
                .HasComment("Selling price of a single product.")
                .HasColumnType("money");
            builder.Property(e => e.UnitPriceDiscount)
                .HasComment("Discount amount.")
                .HasColumnType("money");

            builder.HasOne(d => d.SalesOrder).WithMany(p => p.SalesOrderDetails).HasForeignKey(d => d.SalesOrderId);

            builder.HasOne(d => d.Product).WithMany(p => p.SalesOrderDetails).HasForeignKey(d => d.ProductId);
        }
    }
}
