using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public class ApplicationDbInitialize
{
    public static void Initialize(IServiceProvider servicesProvider)
    {
        var context = servicesProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
}
