using CRM.API.Model.Tenant;
using Microsoft.Extensions.Options;

public class UserService
{
    private readonly TenantDbContext _db;
    private readonly TenantContext _tenant;
    private readonly IOptionsMonitor<Dictionary<string, SubscriptionPlan>> _plans;

    public UserService(TenantDbContext db,
        TenantContext tenant,
        IOptionsMonitor<Dictionary<string, SubscriptionPlan>> plans)
    {
        _db = db;
        _tenant = tenant;
        _plans = plans;
    }

    public async Task CreateUserAsync(ApplicationUser user)
    {
        var tenant = _tenant.Tenant!;
        var plan = _plans.CurrentValue[tenant.SubscriptionType];

        var userCount = await _db.Users.CountAsync();

        if (userCount >= plan.MaxUsers)
            throw new Exception("User limit reached for your subscription");

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}
