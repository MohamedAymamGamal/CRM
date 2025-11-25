using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Model.Tenant;

namespace CRM.API.Data
{
    public class LandlordDbContext : DbContext
    {
        public LandlordDbContext(DbContextOptions<LandlordDbContext> options) : base(options)
        {
        }
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    }
}