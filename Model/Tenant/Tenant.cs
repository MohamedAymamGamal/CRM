using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Model.Tenant
{

    public class Tenant
    {
         public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string SubscriptionType { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public string DatabaseConnectionString { get; set; } = default!;
    public string HostName { get; set; } = default!; 
    }

}
