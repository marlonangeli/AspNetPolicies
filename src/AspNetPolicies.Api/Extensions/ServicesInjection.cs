using AspNetPolicies.Data.Repositories;
using AspNetPolicies.Domain.Interfaces;

namespace AspNetPolicies.Api.Extensions;

public static class ServicesInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
    }
}