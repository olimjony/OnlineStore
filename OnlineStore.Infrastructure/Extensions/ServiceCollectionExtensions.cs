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
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using OnlineStore.Infrastructure.Services.EmailService;
using Domain.DTOs.EmailDTOs;
namespace OnlineStore.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        var connectionString = configuration.GetConnectionString("PGConnectionString");

        //domain services
        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ISeeder, Seeder>();

        // validators
        services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterValidator>();
        services.AddScoped<IValidator<UserLoginDTO>, UserLoginValidator>(); 
        services.AddScoped<IValidator<CreateMarketplaceDTO>, MarketplaceValidator>();
        services.AddScoped<IValidator<CreateProductDTO>, ProductValidator>();
        services.AddScoped<IValidator<CartDTO>, CartValidator>();

        //other libs
        services.AddAutoMapper(typeof(Automapper));

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig!);

        //custom services
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();

        //repositories
        services.AddScoped<IUserAuthentication, UserAutentication>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMarketplaceService, MarketplaceService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICartProductService, CartProductService>();
    }
}
