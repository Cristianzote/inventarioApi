using Microsoft.EntityFrameworkCore;
using inventarioApi.Data.Models;


namespace InventarioApi;

public class InventarioContext: DbContext
{
    //Context
    public InventarioContext(DbContextOptions<InventarioContext> options)
        : base(options)
    {

    }
    //Tables
    //public DbSet<UserInventoryType> UserInventoryTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Presentation> Presentations { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    //public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }

    //ModelBuilder
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //PRODUCT
        List<Product> productInit = new List<Product>();
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("PRODUCT");
            product.HasKey(i => i.IdProduct);
            product.HasIndex(i => i.IdProduct).IsUnique();

            product.Property(i => i.Name).IsRequired();
            product.Property(i => i.Description).IsRequired();
            product.Property(i => i.Category).IsRequired();
            product.Property(i => i.Image).IsRequired();
            product.Property(i => i.Date);

            product.HasMany(i => i.Presentations)
                .WithOne(p => p.Products)
                .HasForeignKey(p => p.Product);

            product.HasData(productInit);
        });

        //PRESENTATION
        List<Presentation> presentationInit = new List<Presentation>();
        modelBuilder.Entity<Presentation>(presentation =>
        {
            presentation.ToTable("PRESENTATION");
            presentation.HasKey(i => i.IdPresentation);
            presentation.HasIndex(i => i.IdPresentation).IsUnique();

            presentation.Property(i => i.Name).IsRequired();
            presentation.Property(i => i.Description).IsRequired();
            presentation.Property(i => i.Quantity).IsRequired();
            presentation.Property(i => i.PriceIncome).IsRequired();
            presentation.Property(i => i.PriceOutput).IsRequired();
            presentation.Property(i => i.PriceRetail).IsRequired();
            presentation.Property(i => i.Stock).IsRequired();
            presentation.Property(i => i.RetailStock).IsRequired();
            presentation.Property(i => i.RetailStockRatio).IsRequired();
            presentation.Property(i => i.Date);
            presentation.Property(i => i.Product).IsRequired();

            presentation.HasData(presentationInit);
        });

        //TRANSACTION
        List<Transaction> transactionInit = new List<Transaction>();
        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.ToTable("TRANSACTION");
            transaction.HasKey(i => i.IdTransaction);
            transaction.HasIndex(i => i.IdTransaction).IsUnique();

            transaction.Property(i => i.Value).IsRequired();
            transaction.Property(i => i.Type).IsRequired();
            transaction.Property(i => i.Date);

            transaction.HasMany(i => i.TransactionDetail)
                .WithOne(i => i.Transactions)
                .HasForeignKey(i => i.Transaction);

            transaction.HasData(transactionInit);
        });

        //TRANSACTION_DETAILS
        List<TransactionDetail> transactionDetailInit = new List<TransactionDetail>();
        modelBuilder.Entity<TransactionDetail>(transactionDetail =>
        {
            transactionDetail.ToTable("TRANSACTION_DETAIL");
            transactionDetail.HasKey(i => i.IdTransactionDetail);
            transactionDetail.HasIndex(i => i.IdTransactionDetail).IsUnique();

            transactionDetail.Property(i => i.Quantity).IsRequired();
            transactionDetail.Property(i => i.Detail).IsRequired();
            transactionDetail.Property(i => i.Presentation).IsRequired();
            transactionDetail.Property(i => i.Transaction).IsRequired();

            //transactionDetail.Property(i => i.DATE);

            transactionDetail.HasData(transactionDetailInit);
        });
    }
}
