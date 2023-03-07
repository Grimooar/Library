﻿namespace WebApi.NewDbContext;

public class ApplicationDbInitialize
{
    public static void Initialize(IServiceProvider servicesProvider)
    {
        var context = servicesProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
    }
}
