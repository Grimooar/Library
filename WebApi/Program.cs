
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kirel.Repositories.Infrastructure.Generics;
using Kirel.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApi.Controllers;
using WebApi.DbContext;
using WebApi.Models;
using WebApi.NewDbContext;


using WebApi.Service;
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
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, BookInfo>, KirelGenericEntityFrameworkRepository<int, BookInfo, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Author>, KirelGenericEntityFrameworkRepository<int, Author, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Publisher>, KirelGenericEntityFrameworkRepository<int, Publisher, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, BookInStock>, KirelGenericEntityFrameworkRepository<int, BookInStock, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, Book>, KirelGenericEntityFrameworkRepository<int, Book, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int, User>, KirelGenericEntityFrameworkRepository<int, User, ApplicationDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int,BookInAbonement>, KirelGenericEntityFrameworkRepository<int, BookInAbonement, DataDbContext>>();
        builder.Services.AddScoped<IKirelGenericEntityFrameworkRepository<int,Abonement>, KirelGenericEntityFrameworkRepository<int, Abonement, DataDbContext>>();
        
        builder.Services.AddIdentity<User, IdentityRole<int>>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddSingleton(authOptions);
        
    
       builder.Services.AddScoped<AuthService>();
       builder.Services.AddScoped<BookInStockService>();
        builder.Services.AddScoped<PublisherService>();
        builder.Services.AddScoped<BookInfoService>();
        builder.Services.AddScoped<BookService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<BookInAbonementService>();
        builder.Services.AddScoped<AuthorService>();
        builder.Services.AddScoped<AbonementService>();
        
        
        
        
        

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            if (authOptions.Key != null)
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = authOptions.GetSymmetricSecurityKey(authOptions.Key),
                    ValidateIssuerSigningKey = true
                };
        });
        
        builder.Services.AddControllers();
        // Add services to the container.
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Add dto validators. Configured in Validators.
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        
        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
            {  
                Name = "Authorization",  
                Type = SecuritySchemeType.ApiKey,  
                Scheme = "Bearer",  
                BearerFormat = "JWT",  
                In = ParameterLocation.Header,  
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",  
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement  
            {  
                {  
                    new OpenApiSecurityScheme  
                    {  
                        Reference = new OpenApiReference  
                        {  
                            Type = ReferenceType.SecurityScheme,  
                            Id = "Bearer"  
                        }  
                    },  
                    new string[] {}
                }  
            });
        });
       
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
    
   

}

