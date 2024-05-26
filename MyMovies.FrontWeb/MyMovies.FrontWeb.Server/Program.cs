using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyMovies.FrontWeb.Server.Data;
using MyMovies.FrontWeb.Server.Extensions;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Ajout du Client HttpClient
builder.Services.AddHttpClient();

// Add services to the container.

// Ajout de la configuration d'Entity Framework
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationIdentityDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationIdentityDbContext' not found.")));


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajout de l'Authentification utilisant le cookie aspnet
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontAccess",
        policy =>
        {
            policy.WithOrigins("https://localhost:5173").AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*");
            //policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("*");
        });
});

// Configuration du EndPoint IdentityApi
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Utilisation des ervices Authentification et d'autorisation
app.UseAuthentication();
app.UseAuthorization();

// Ajout du Mapping de l'Api Identity
app.MapGroup("/api/account").MapIdentityApi<ApplicationUser>();

app.MapFallbackToFile("/index.html");

// Utilisation des policies Cors
app.UseCors("FrontAccess");

// Ajout du Mapping de l'API Movies
app.MapMoviesApi();

app.Run();

