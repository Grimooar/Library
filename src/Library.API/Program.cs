using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Library.Core.Extentions;
using Library.Infrastructure;
using Library.Infrastructure.Extentions;
using Library.Models;
using Library.Shared;


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
        
        builder.Services.AddRepositories();



    

    builder.Services.AddMapper();
        builder.Services.AddServices();
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
    
    
    /*public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(); // Make sure you call this previous to AddMvc
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Make sure you call this before calling app.UseMvc()
        app.UseCors(
            options => options.WithOrigins("http://localhost:7194").AllowAnyMethod()
        );*/
       
    }




