using IWantApp.Domain.Products;

namespace IWantApp.Infra.Database;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ignorando propriedades das notifications
        builder.Ignore<Notification>();

        // impondo regras de propriedades
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(255);
        builder.Entity<Product>().Property(p => p.Name).IsRequired();

        builder.Entity<Category>().Property(c => c.Name).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        // generaliza campos string com no máximo 100 chars
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
