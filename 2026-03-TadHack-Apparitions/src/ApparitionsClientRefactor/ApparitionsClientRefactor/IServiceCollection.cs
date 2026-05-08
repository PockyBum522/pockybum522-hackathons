using ApparitionsClientRefactor.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ApparitionsClientRefactor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApparitionsClientCore(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        
        return services;
    }
}