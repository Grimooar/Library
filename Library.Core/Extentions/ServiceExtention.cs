using Library.Core.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Core.Extentions;

public static class ServiceExtention
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<BookInStockService>();
        services.AddScoped<PublisherService>();
        services.AddScoped<BookInfoService>();
        services.AddScoped<BookService>();
        services.AddScoped<BookInAbonementService>();
        services.AddScoped<AuthorService>();
        services.AddScoped<AbonementService>();
        
        
    } 
    

}