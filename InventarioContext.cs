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
    public DbSet<Expense> Expences { get; set; }
    public DbSet<MonthlyRegister> MonthlyRegister { get; set; }
    public DbSet<MonthlyExpense> MonthlyExpences { get; set; }

    //ModelBuilder
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //PRODUCT
        List<Product> productInit = new List<Product>();
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("PRODUCT");
            product.HasKey(p => p.IdProduct);
            product.HasIndex(p => p.IdProduct).IsUnique();

            product.Property(p => p.Name).IsRequired();
            product.Property(p => p.Description).IsRequired();
            product.Property(p => p.Category).IsRequired();
            product.Property(p => p.Image).IsRequired();
            product.Property(p => p.Date);

            product.HasMany(p => p.Presentations)
                .WithOne(p => p.Products)
                .HasForeignKey(p => p.Product);

            product.HasData(productInit);
        });

        //PRESENTATION
        List<Presentation> presentationInit = new List<Presentation>();
        modelBuilder.Entity<Presentation>(presentation =>
        {
            presentation.ToTable("PRESENTATION");
            presentation.HasKey(pr => pr.IdPresentation);
            presentation.HasIndex(pr => pr.IdPresentation).IsUnique();

            presentation.Property(pr => pr.Name).IsRequired();
            presentation.Property(pr => pr.Description).IsRequired();
            presentation.Property(pr => pr.PriceIncome).IsRequired();
            presentation.Property(pr => pr.PriceOutput).IsRequired();
            presentation.Property(pr => pr.PriceOutputCover).IsRequired();
            presentation.Property(pr => pr.PriceRetail).IsRequired();
            presentation.Property(pr => pr.PriceRetailCover).IsRequired();
            presentation.Property(pr => pr.Stock).IsRequired();
            presentation.Property(pr => pr.RetailStock).IsRequired();
            presentation.Property(pr => pr.RetailStockRatio).IsRequired();
            presentation.Property(pr => pr.Date);
            presentation.Property(pr => pr.Product).IsRequired();

            presentation.HasData(presentationInit);
        });

        //TRANSACTION
        List<Transaction> transactionInit = new List<Transaction>();
        modelBuilder.Entity<Transaction>(transaction =>
        {
            transaction.ToTable("TRANSACTION");
            transaction.HasKey(t => t.IdTransaction);
            transaction.HasIndex(t => t.IdTransaction).IsUnique();

            transaction.Property(t => t.Value).IsRequired();
            transaction.Property(t => t.Type).IsRequired();
            transaction.Property(t => t.Date);
            transaction.Property(t => t.Cover);
            transaction.Property(t => t.Table);

            transaction.HasMany(t => t.TransactionDetail)
                .WithOne(t => t.Transactions)
                .HasForeignKey(t => t.Transaction);

            transaction.HasData(transactionInit);
        });

        //TRANSACTION_DETAILS
        List<TransactionDetail> transactionDetailInit = new List<TransactionDetail>();
        modelBuilder.Entity<TransactionDetail>(transactionDetail =>
        {
            transactionDetail.ToTable("TRANSACTION_DETAIL");
            transactionDetail.HasKey(td => td.IdTransactionDetail);
            transactionDetail.HasIndex(td => td.IdTransactionDetail).IsUnique();

            transactionDetail.Property(td => td.Quantity).IsRequired();
            transactionDetail.Property(td => td.Detail).IsRequired();
            transactionDetail.Property(td => td.Presentation).IsRequired();
            transactionDetail.Property(td => td.Transaction).IsRequired();

            transactionDetail.HasData(transactionDetailInit);
        });

        //EXPENSES
        List<Expense> expencesInit = new List<Expense>();
        modelBuilder.Entity<Expense>(expences =>
        {
            expences.ToTable("EXPENSE");
            expences.HasKey(e => e.IdExpense);
            expences.HasIndex(e => e.IdExpense).IsUnique();

            expences.Property(e => e.Value).IsRequired();
            expences.Property(e => e.Name).IsRequired();
            expences.Property(e => e.Description).IsRequired();
            expences.Property(e => e.Type).IsRequired();
            expences.Property(e => e.Active).IsRequired();
            expences.Property(e => e.Multiplier).IsRequired();

            expences.HasData(expencesInit);
        });

        //MONTHLY_REGISTER
        List<MonthlyRegister> monthlyRegisterInit = new List<MonthlyRegister>();
        modelBuilder.Entity<MonthlyRegister>(monthlyRegister =>
        {
            monthlyRegister.ToTable("MONTHLY_REGISTER");
            monthlyRegister.HasKey(mr => mr.IdMonthlyRegister);
            monthlyRegister.HasIndex(mr => mr.IdMonthlyRegister).IsUnique();

            monthlyRegister.Property(mr => mr.InitialDate).IsRequired();
            monthlyRegister.Property(mr => mr.InitialDate).IsRequired();
            monthlyRegister.Property(mr => mr.InitialInventory).IsRequired();
            monthlyRegister.Property(mr => mr.FinalInventory).IsRequired();
            monthlyRegister.Property(mr => mr.Purchases).IsRequired();
            monthlyRegister.Property(mr => mr.Expenses).IsRequired();

            monthlyRegister.HasData(monthlyRegisterInit);
        });

        //MONTHLY_EXPENCES
        List<MonthlyExpense> monthlyExpencesInit = new List<MonthlyExpense>();
        modelBuilder.Entity<MonthlyExpense>(monthlyExpences =>
        {
            monthlyExpences.ToTable("MONTHLY_EXPENSE");
            monthlyExpences.HasKey(me => me.IdMonthlyExpenses);
            monthlyExpences.HasIndex(me => me.IdMonthlyExpenses).IsUnique();

            monthlyExpences.Property(me => me.Expense).IsRequired();
            monthlyExpences.Property(me => me.MonthlyRegister).IsRequired();

            monthlyExpences.HasData(transactionDetailInit);
        });
    }
}
