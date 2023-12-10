
using System.Configuration;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kirel.Repositories.Infrastructure.Generics;
using Kirel.Repositories.Interfaces;
using Library.Core.Extentions;
using Library.Core.Service;
using Library.Infrastructure;
using Library.Infrastructure.Extentions;
using Library.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extentions;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;


namespace WebApi;

class Program
{ 
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("JWTSettings"));
        var configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
        var authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
        if (authOptions == null)
        {
            throw new Exception("AuthOptions configuration section is missing or invalid.");
        }
        
        //Add AutoMapper. Class <--> Dto mappings. Configured in Mappings.
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // builder.Services.AddSingleton(authOption);
       
        builder.Services.AddDbContext<DataDbContext>(options => options.UseSqlite("Filename=MyTestedDb.db"));
        builder.Services.AddDbContext<ApplicationDbContext>(db => db.UseSqlite("Data Source=ApplicationDb.db"));
        
        builder.Services.AddRepositories();
        builder.Services.AddIdentity<User, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        builder.Services.AddMapper();
        
        builder.Services.AddSingleton(authOptions);
        builder.Services.AddSwagger(authOptions);
        // Add dto validators. Configured in Validators.
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        
        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddControllers();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DevCorsPolicy", corsBuilder =>
            {
                corsBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders();
            });
        });
        var app = builder.Build(); 
        DataDbInitializer.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
        ApplicationDbInitialize.Initialize(app.Services.GetRequiredService<IServiceProvider>().CreateScope().ServiceProvider);
      
        app.UseAuthentication();
        app.UseCors("DevCorsPolicy");
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

      
     

        

       
        
        app.MapControllers();

        app.Run();
    }
    
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(); // Make sure you call this previous to AddMvc
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Make sure you call this before calling app.UseMvc()
        app.UseCors(
            options => options.WithOrigins("http://localhost:7194").AllowAnyMethod()
        );
       
    }


}

