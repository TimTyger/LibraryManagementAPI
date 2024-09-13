using AutoMapper;
using LibraryApi_Repository.Data;
using LibraryApi_Repository.Interfaces;
using LibraryApi_Repository.Models;
using LibraryApi_Repository.Repositories;
using LibraryAPI_Service;
using LibraryAPI_Service.Enums;
using LibraryAPI_Service.Helpers;
using LibraryAPI_Service.Interfaces;
using LibraryAPI_Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var assembly = AppDomain.CurrentDomain.GetAssemblies();
// Add services to the container.
builder.Services.AddDbContext<LibraryApiContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(assembly);
builder.Services.AddSingleton(mapper => new MapperConfiguration(x=>
{
    x.AddProfile(new MapperProfile(mapper.GetRequiredService<IHttpContextAccessor>(), assembly));
}).CreateMapper()
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = Configuration["Google:client_id"] ?? "";
    googleOptions.ClientSecret = Configuration["Google:client_secret"] ?? "";
    googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        Console.WriteLine($"Redirecting to: {context.RedirectUri}");
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AppOwner", policy => policy.RequireRole(Roles.AppOwner.ToString()))
    .AddPolicy("Customer", policy => policy.RequireRole(Roles.Customer.ToString()));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();
builder.Services.AddScoped<INotificationRepo, NotificationRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    var xmlFile = $"{AppDomain.CurrentDomain.FriendlyName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<PerformanceLogger>();
app.UseMiddleware<ErrorHandler>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request path: {context.Request.Path}");
    await next();
});

app.MapControllers();


app.Run();
