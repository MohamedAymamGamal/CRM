using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Interface;

namespace CRM.API.middleware
{
    public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, 
        ITenantResolver  resolver, 
        TenantContext tenantContext)
    {
        var tenant = await resolver.ResolveTenantAsync(context);

        if (tenant is null)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Tenant not found");
            return;
        }

        if (!tenant.IsActive || tenant.ExpiresAt < DateTime.UtcNow)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Subscription expired");
            return;
        }

        tenantContext.Tenant = tenant; // store

        await _next(context);
    }
}
}