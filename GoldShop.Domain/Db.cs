using GoldShop.Domain.Entity;
using GoldShop.Domain.Entity.Factor;
using GoldShop.Domain.Entity.Page;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Domain;

public class Db : IdentityDbContext<User, Role, long>
{
    public Db(DbContextOptions<Db> options) : base(options)
    {
    }

    public Db()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Data Source=185.88.153.67,1430;Initial Catalog=Yaas;User ID=user_Dev;Password=Keyvan13800!;Trust Server Certificate=True");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<State> States { set; get; }
    public DbSet<City> Cities { set; get; }
    public DbSet<Category> Categories { set; get; }
    public DbSet<Factor> Factors { set; get; }
    public DbSet<Product> Products { set; get; }
    public DbSet<FactorProduct> FactorProducts { set; get; }
    public DbSet<UserAddress> UserAddresses { set; get; }
    public DbSet<PostMethod> PostMethods { set; get; }
    public DbSet<ContactUs>  ContactUs { set; get; }
    public DbSet<DiscountCode>  DiscountCodes { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<State>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Factor>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<FactorProduct>().HasQueryFilter(x => !x.IsDelete);
        base.OnModelCreating(modelBuilder);
    }
}