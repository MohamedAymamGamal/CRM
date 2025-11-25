using CRM.API.Model.Tenant;

public class TenantDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly TenantContext _tenantContext;

    public TenantDbContext(
        DbContextOptions<TenantDbContext> options,
        TenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenant = _tenantContext.Tenant;

        if (tenant != null)
        {
            optionsBuilder.UseSqlServer(tenant.DatabaseConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
          builder.Entity<OrderItem>()
               .HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId);

        builder.Entity<OrderItem>()
               .HasOne(oi => oi.Product)
               .WithMany(p => p.OrderItems)
               .HasForeignKey(oi => oi.ProductId);
    }

    // Tenant-specific tables
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Product> Products => Set<Product>();
}
