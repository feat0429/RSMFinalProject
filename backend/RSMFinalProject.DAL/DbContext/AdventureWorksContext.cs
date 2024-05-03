
namespace RSMFinalProject.DAL.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using RSMFinalProject.Model;
    using System.Reflection;

    public partial class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext()
        {
        }

        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<ProductSubcategory> ProductSubcategories { get; set; }

        public virtual DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

        public virtual DbSet<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual DbSet<SalesTerritory> SalesTerritories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}

