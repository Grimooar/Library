using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Core.Extentions;

/// </summary>
public static class AutoMapperExtension
{
    /// <summary>
    /// </summary>
    /// <param name="services"> </param>
    public static void AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}