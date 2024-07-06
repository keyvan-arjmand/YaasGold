using GoldShop.Domain.Entity;
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
            "Data Source=DESKTOP-M202FR8\\KEY1;Initial Catalog=YassGold;Integrated Security=True;Trust Server Certificate=True");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<State> States { set; get; }
    public DbSet<City> Cities { set; get; }
    public DbSet<Category> Categories { set; get; }
    public DbSet<Factor> Factors { set; get; }
    public DbSet<Product> Products { set; get; }
    public DbSet<ProductFactor> ProductFactors { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<State>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<City>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Factor>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDelete);
        modelBuilder.Entity<ProductFactor>().HasQueryFilter(x => !x.IsDelete);
        base.OnModelCreating(modelBuilder);
    }
}