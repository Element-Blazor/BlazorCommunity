using Microsoft.Extensions.DependencyInjection;

namespace ElementCommunity.Infrastructure;

public static class ElementCommunityInfrastructureExtensions
{
    public static IServiceCollection AddElementCommunityInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICommunityReadModel, StaticCommunityReadModel>();
        return services;
    }
}
