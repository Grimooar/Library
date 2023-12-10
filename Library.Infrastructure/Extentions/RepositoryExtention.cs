using Kirel.Repositories.Infrastructure.Generics;
using Kirel.Repositories.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Extentions;

public static class RepositoryExtention
{
    public static void AddRepositories(this IServiceCollection services)
    {
       services.AddDbContext<ApplicationDbContext>(db => db.UseSqlite("Data Source=ApplicationDb.db"));
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, BookInfo>, KirelGenericEntityFrameworkRepository<int, BookInfo, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Author>, KirelGenericEntityFrameworkRepository<int, Author, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Publisher>, KirelGenericEntityFrameworkRepository<int, Publisher, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, BookInStock>, KirelGenericEntityFrameworkRepository<int, BookInStock, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Book>, KirelGenericEntityFrameworkRepository<int, Book, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int, User>, KirelGenericEntityFrameworkRepository<int, User, ApplicationDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int,BookInAbonement>, KirelGenericEntityFrameworkRepository<int, BookInAbonement, DataDbContext>>();
        services.AddScoped<IKirelGenericEntityFrameworkRepository<int,Abonement>, KirelGenericEntityFrameworkRepository<int, Abonement, DataDbContext>>();

    }
}