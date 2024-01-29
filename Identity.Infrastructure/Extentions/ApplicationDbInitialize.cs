using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Extentions;

public class ApplicationDbInitialize
{
    public static void Initialize(IServiceProvider servicesProvider)
    {
        var context = servicesProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
}
