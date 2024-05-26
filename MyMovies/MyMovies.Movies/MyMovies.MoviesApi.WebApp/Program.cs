using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MyMovies.MoviesApi.WebApp.Routing;
using MyMovies.MoviesApi.WebApp.Services;
using MyMovies.MoviesLibrary.Business.Services;
using MyMovies.MoviesLibrary.Business.Services.Impl;
using MyMovies.MoviesLibrary.Data.Data;
using MyMovies.MoviesLibrary.Data.Migrations;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Data.Repository.Impl;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Ajout de la gestion des Controllers
builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddScoped<MovieFeedService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Movies API",
        Description = "Web API qui permet de gerer des films",
        TermsOfService = new Uri("https://www.onecode.fr/terms"),
        Contact = new OpenApiContact
        {
            Name = "Olivier Navarre",
            Url = new Uri("https://www.onecode.fr/contact")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://www.onecode.fr/license")
        }

    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Flows = new OpenApiOAuthFlows()
        {
            ClientCredentials = new OpenApiOAuthFlow()
            {
                TokenUrl = new Uri($"https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>(){
                    {"moviesapi", "moviesapi"}
                }
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllAccess",
        builder =>
        {
            builder.WithOrigins("https://localhost:5173").AllowAnyMethod().AllowAnyHeader();
        });
});

// Configuration du Service Collection pour Fluent Migrator
builder.Services.AddSingleton<DapperContext>()
    .AddFluentMigratorCore()
    .ConfigureRunner(options =>
    {
        options.AddSqlServer2014()
        .ScanIn(typeof(DapperContext).Assembly).For.All()
        .WithGlobalConnectionString(serviceProvider => serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("SqlConnectionString"));
    })
    .AddSingleton<Database>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //options.SwaggerEndpoint("/swagger/v1/moviesAPI.json", "V1");
        //options.RoutePrefix = string.Empty;
        options.OAuthClientId("bbfapplication");
        options.OAuthClientSecret("bbfapplication");
        options.DisplayRequestDuration();
    }
    );
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Utilisation des policies Cors
app.UseCors("AllAccess");

// Ajout du Routing sur les controllers
//app.MapControllerRoute(
//    name: "default",
//    pattern: "news/movies/{strDatetime}",
//    defaults: new { controller = "News", action="Movies"},
//    constraints: new { strDatetime = @"^\d{2}-\d{2}-\d{4}$" }
//    );

app.MapControllerRoute(
    name: "default",
    pattern: "news/movies/{strDatetime}",
    defaults: new { controller = "News", action = "Movies" },
    constraints: new { isOK = new DateFormatConstraint() }
    );

//app.MapDefaultControllerRoute();

app.MapControllers();

// Execution des Migrations
var config = app.Services.GetService<IConfiguration>();
var sqlConnectionString = app.Services.GetService<IConfiguration>()!.GetConnectionString("SqlConnectionString");
SqlConnectionStringBuilder sqlConnectionStringBuilder = new(sqlConnectionString);

app.MigrateDatabase(sqlConnectionStringBuilder.InitialCatalog);

app.Run();




