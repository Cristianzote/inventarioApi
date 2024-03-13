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
    public DbSet<User> Users { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<UserInventory> UserInventories { get; set; }
    //public DbSet<UserInventoryType> UserInventoryTypes { get; set; }
    //public DbSet<Transaction> Transactions { get; set; }
    //public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    //public DbSet<Presentation> Presentations { get; set; }
    //public DbSet<TransactionDetail> TransactionDetails { get; set; }

    //ModelBuilder
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //User
        List<User> usersInit = new List<User>();
        //usersInit.Add(new User() { ID_USER = 123, NAME = "Cristian prueba 1", EMAIL = "cristiancitoowo@gmail.com", DATE = DateTime.Now, UUID = '550e8400-e29b-41d4-a716-446655440000' });
        modelBuilder.Entity<User>(user =>
        {
            user.ToTable("USER");
            user.HasKey(u => u.ID_USER);
            user.HasIndex(u => u.ID_USER).IsUnique();

            user.Property(u => u.NAME).IsRequired().HasMaxLength(150);
            user.Property(u => u.EMAIL).IsRequired(false);
            user.Property(u => u.DATE);

            user.HasData(usersInit);
        });


        //Inventory
        List<Inventory> inventoryInit = new List<Inventory>();
        modelBuilder.Entity<Inventory>(inventory =>
        {
            inventory.ToTable("INVENTORY");
            inventory.HasKey(i => i.ID_INVENTORY);
            inventory.HasIndex(i => i.ID_INVENTORY).IsUnique();

            inventory.Property(i => i.TITLE).IsRequired().HasMaxLength(150);
            inventory.Property(i => i.DESCRIPTION).IsRequired().HasMaxLength(150);
            inventory.Property(i => i.IMAGE).IsRequired(false);
            inventory.Property(i => i.DATE);

            inventory.HasData(inventoryInit);
        });


        //UserInventory
        List<UserInventory> UserInventoryInit = new List<UserInventory>();
        modelBuilder.Entity<UserInventory>(userInventory =>
        {
            userInventory.ToTable("USER_INVENTORY");
            userInventory.HasKey(i => i.ID_USER_INVENTORY);
            userInventory.HasIndex(i => i.ID_USER_INVENTORY).IsUnique();

            userInventory.Property(i => i.USER).IsRequired();
            userInventory.Property(i => i.INVENTORY).IsRequired();
            userInventory.Property(i => i.TYPE).IsRequired();
            userInventory.Property(i => i.DATE);

            userInventory.HasData(UserInventoryInit);
        });

        //PRODUCT
        List<Product> ProductInit = new List<Product>();
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("PRODUCT");
            product.HasKey(i => i.ID_PRODUCT);
            product.HasIndex(i => i.ID_PRODUCT).IsUnique();

            product.Property(i => i.NAME).IsRequired();
            product.Property(i => i.DESCRIPTION).IsRequired();
            product.Property(i => i.IMAGE).IsRequired();
            product.Property(i => i.DATE);
            //product.Property(i => i.INVENTORY).IsRequired();

            product.HasData(ProductInit);
        });
    }
}
