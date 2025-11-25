using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Data;
using CRM.API.Model.Tenant;

namespace CRM.API.Service
{
    public interface ITenantResolver
{
    Task<Tenant?> ResolveTenantAsync(HttpContext context);
}
    public class TenantResolver : ITenantResolver
    {
        private readonly LandlordDbContext _landlord;

        public TenantResolver(LandlordDbContext landlord)
        {
            _landlord = landlord;
        }

        public async Task<Tenant?> ResolveTenantAsync(HttpContext context)
        {
            // Example: subdomain tenant1.myapp.com
            var host = context.Request.Host.Host;

            return await _landlord.Tenants
                .FirstOrDefaultAsync(t => t.HostName == host);
        }
    }
}