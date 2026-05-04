using Microsoft.Extensions.DependencyInjection;
using ApparitionsClient.ViewModels;
namespace ApparitionsClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApparitionsClientCore(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        
        return services;
    }
}