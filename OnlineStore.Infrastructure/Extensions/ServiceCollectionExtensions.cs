using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure.Seeders;
using OnlineStore.Application.Interfaces;
using OnlineStore.Infrastructure.Repositories;
using FluentValidation;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Validators;
using OnlineStore.Application.Mapping;
namespace OnlineStore.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        var connectionString = configuration.GetConnectionString("PGConnectionString");
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ISeeder, Seeder>();
        services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterValidator>();
        services.AddScoped<IValidator<UserLoginDTO>, UserLoginValidator>(); 
        services.AddScoped<IValidator<MarketplaceDTO>, MarketplaceValidator>();
        services.AddScoped<IValidator<ProductDTO>, ProductValidator>();
        services.AddScoped<IValidator<CartDTO>, CartValidator>();
        services.AddAutoMapper(typeof(Automapper));
        services.AddScoped<IUserAuthentication, UserAutentication>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMarketplaceService, MarketplaceService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartProductService, CartProductService>();
    }
}
