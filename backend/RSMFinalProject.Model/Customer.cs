namespace RSMFinalProject.Model;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int? PersonId { get; set; }

    public int? StoreId { get; set; }

    public int? TerritoryId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public Guid Rowguid { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual Person? Person { get; set; }

    public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; } = new List<SalesOrderHeader>();
}
