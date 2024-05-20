using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using MyMovies.MoviesApi.WebApp.Services;
using MyMovies.MoviesLibrary.Data.Data;
using MyMovies.MoviesLibrary.Data.Migrations;
using MyMovies.MoviesLibrary.Data.Repository;
using MyMovies.MoviesLibrary.Data.Repository.impl;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "API V1",
        Title = "API Movies",
        Description = "Web API permettant de gerer les films",
        Contact = new OpenApiContact { Name = "FisherX" },
        License = new OpenApiLicense { Name = "Licence" }
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddScoped<IMovieFeedService, MovieFeedService>();

builder.Services.AddScoped<IMovieRepository, MovieRepository>();


builder.Services.AddSingleton<Database>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddFluentMigratorCore().ConfigureRunner(options =>
{
    options.AddSqlServer2014().ScanIn(typeof(DapperContext).Assembly).For.All()
    .WithGlobalConnectionString(serviceProvider =>
     serviceProvider.GetService<IConfiguration>()!.GetConnectionString("SqlConnectionString"));
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("dateFormat", typeof(DateFormatConstraint));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

//app.UseHttpsRedirection();

//app.MapDefaultControllerRoute();

//app.MapControllerRoute("default", "{controller=Actors}/{action=Get}/{id?}");

//app.MapControllerRoute("News", "{controller}/{action}/{publishedDate?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Actors}/{action=Get}/{id?}");

//app.MapControllerRoute(
//    name: "News",
//    pattern: "news/movies/{strDatetime?}",
//    defaults: new { controller = "News", action = "Get" },
//    //constraints: new { strDatetime = @"^\d{4}-\d{2}-\d{2}$" }
//    constraints: new { isOk = new DateFormatConstraint() }
//    );


// app.MapControllerRoute("test", "{controller}/{action}/{publishedDate?}");


app.MapControllers();

Console.WriteLine(app.Configuration["test"]);

string connectionString = app.Services.GetService<IConfiguration>()!.GetConnectionString("SqlConnectionString")!;
SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
app.MigrationDatabase(sqlConnectionStringBuilder.InitialCatalog);

app.Run();

