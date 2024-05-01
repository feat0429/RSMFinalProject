namespace RSMFinalProject.DAL.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using RSMFinalProject.Model;

    public class SalesOrderHeaderConfiguration : IEntityTypeConfiguration<SalesOrderHeader>
    {
        public void Configure(EntityTypeBuilder<SalesOrderHeader> builder)
        {
            builder.HasKey(e => e.SalesOrderId).HasName("PK_SalesOrderHeader_SalesOrderID");

            builder.ToTable("SalesOrderHeader", "Sales", tb =>
            {
                tb.HasComment("General sales order information.");
                tb.HasTrigger("uSalesOrderHeader");
            });

            builder.HasIndex(e => e.SalesOrderNumber, "AK_SalesOrderHeader_SalesOrderNumber").IsUnique();

            builder.HasIndex(e => e.Rowguid, "AK_SalesOrderHeader_rowguid").IsUnique();

            builder.HasIndex(e => e.CustomerId, "IX_SalesOrderHeader_CustomerID");

            builder.HasIndex(e => e.SalesPersonId, "IX_SalesOrderHeader_SalesPersonID");

            builder.Property(e => e.SalesOrderId)
                .HasComment("Primary key.")
                .HasColumnName("SalesOrderID");
            builder.Property(e => e.AccountNumber)
                .HasMaxLength(15)
                .HasComment("Financial accounting number reference.");
            builder.Property(e => e.BillToAddressId)
                .HasComment("Customer billing address. Foreign key to Address.AddressID.")
                .HasColumnName("BillToAddressID");
            builder.Property(e => e.Comment)
                .HasMaxLength(128)
                .HasComment("Sales representative comments.");
            builder.Property(e => e.CreditCardApprovalCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasComment("Approval code provided by the credit card company.");
            builder.Property(e => e.CreditCardId)
                .HasComment("Credit card identification number. Foreign key to CreditCard.CreditCardID.")
                .HasColumnName("CreditCardID");
            builder.Property(e => e.CurrencyRateId)
                .HasComment("Currency exchange rate used. Foreign key to CurrencyRate.CurrencyRateID.")
                .HasColumnName("CurrencyRateID");
            builder.Property(e => e.CustomerId)
                .HasComment("Customer identification number. Foreign key to Customer.BusinessEntityID.")
                .HasColumnName("CustomerID");
            builder.Property(e => e.DueDate)
                .HasComment("Date the order is due to the customer.")
                .HasColumnType("datetime");
            builder.Property(e => e.Freight)
                .HasComment("Shipping cost.")
                .HasColumnType("money");
            builder.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.")
                .HasColumnType("datetime");
            builder.Property(e => e.OnlineOrderFlag)
                .HasDefaultValue(true)
                .HasComment("0 = Order placed by sales person. 1 = Order placed online by customer.");
            builder.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Dates the sales order was created.")
                .HasColumnType("datetime");
            builder.Property(e => e.PurchaseOrderNumber)
                .HasMaxLength(25)
                .HasComment("Customer purchase order number reference. ");
            builder.Property(e => e.RevisionNumber).HasComment("Incremental number to track changes to the sales order over time.");
            builder.Property(e => e.Rowguid)
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.")
                .HasColumnName("rowguid");
            builder.Property(e => e.SalesOrderNumber)
                .HasMaxLength(25)
                .HasComputedColumnSql("(isnull(N'SO'+CONVERT([nvarchar](23),[SalesOrderID]),N'*** ERROR ***'))", false)
                .HasComment("Unique sales order identification number.");
            builder.Property(e => e.SalesPersonId)
                .HasComment("Sales person who created the sales order. Foreign key to SalesPerson.BusinessEntityID.")
                .HasColumnName("SalesPersonID");
            builder.Property(e => e.ShipDate)
                .HasComment("Date the order was shipped to the customer.")
                .HasColumnType("datetime");
            builder.Property(e => e.ShipMethodId)
                .HasComment("Shipping method. Foreign key to ShipMethod.ShipMethodID.")
                .HasColumnName("ShipMethodID");
            builder.Property(e => e.ShipToAddressId)
                .HasComment("Customer shipping address. Foreign key to Address.AddressID.")
                .HasColumnName("ShipToAddressID");
            builder.Property(e => e.Status)
                .HasDefaultValue((byte)1)
                .HasComment("Order current status. 1 = In process; 2 = Approved; 3 = Backordered; 4 = Rejected; 5 = Shipped; 6 = Cancelled");
            builder.Property(e => e.SubTotal)
                .HasComment("Sales subtotal. Computed as SUM(SalesOrderDetail.LineTotal)for the appropriate SalesOrderID.")
                .HasColumnType("money");
            builder.Property(e => e.TaxAmt)
                .HasComment("Tax amount.")
                .HasColumnType("money");
            builder.Property(e => e.TerritoryId)
                .HasComment("Territory in which the sale was made. Foreign key to SalesTerritory.SalesTerritoryID.")
                .HasColumnName("TerritoryID");
            builder.Property(e => e.TotalDue)
                .HasComputedColumnSql("(isnull(([SubTotal]+[TaxAmt])+[Freight],(0)))", false)
                .HasComment("Total due from customer. Computed as Subtotal + TaxAmt + Freight.")
                .HasColumnType("money");

            builder.HasOne(d => d.BillToAddress).WithMany(p => p.SalesOrderHeaderBillToAddresses)
                .HasForeignKey(d => d.BillToAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.Customer).WithMany(p => p.SalesOrderHeaders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.SalesPerson).WithMany(p => p.SalesOrderHeaders).HasForeignKey(d => d.SalesPersonId);

            builder.HasOne(d => d.ShipToAddress).WithMany(p => p.SalesOrderHeaderShipToAddresses)
                .HasForeignKey(d => d.ShipToAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
