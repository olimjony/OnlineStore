using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    internal DbSet<UserProfile> UserProfiles { get; set; }
    internal DbSet<User> Users { get; set; }
    internal DbSet<Seller> Sellers { get; set; }
    internal DbSet<Role> Roles { get; set; }
    internal DbSet<UserRole> UserRoles { get; set; }
    internal DbSet<Cart> Carts { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<Category> Categories { get; set; }
    internal DbSet<Product> Products { get; set; }
    internal DbSet<Marketplace> Marketplaces { get; set; }
    internal DbSet<CartProduct> CartProducts { get; set; }
    internal DbSet<ProductImage> ProductImages { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        //UserProfile Relationships
        modelBuilder.Entity<UserProfile>()
            .HasOne(up => up.User)
            .WithOne(u => u.UserProfile)
            .HasForeignKey<User>(u => u.UserProfileId);

        modelBuilder.Entity<UserProfile>()
            .HasOne(up =>up.Seller)
            .WithOne(s => s.UserProfile)
            .HasForeignKey<Seller>(u => u.UserProfileId);

        // User Relationships
        modelBuilder.Entity<User>()
            .HasMany(u => u.Carts)
            .WithOne(c => c.User)
            .HasForeignKey(u => u.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        // User and Seller Roles
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new {ur.UserProfileId, ur.RoleId });
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.UserProfile) 
            .WithMany(up => up.UserRoles) 
            .HasForeignKey(ur => ur.UserProfileId) 
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role) 
            .WithMany(r => r.UserRoles)  
            .HasForeignKey(ur => ur.RoleId) 
            .OnDelete(DeleteBehavior.Cascade);

        // Orders Relationships
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId);
        
        // Cart and Product Relationships    
        modelBuilder.Entity<CartProduct>()
            .HasKey(cp => new { cp.CartId, cp.ProductId });
        modelBuilder.Entity<CartProduct>()
            .HasOne(cp => cp.Cart)
            .WithMany(c => c.CartProducts)
            .HasForeignKey(cp => cp.CartId);
        modelBuilder.Entity<CartProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CartProducts)
            .HasForeignKey(cp => cp.ProductId);
        
        // ProductCategories
        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Связь Category -> Products
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь Category -> CategoryAttributes
        modelBuilder.Entity<Category>()
            .HasMany(c => c.CategoryAttributes)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь Product -> ProductAttributes
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Attributes)
            .WithOne(pa => pa.Product)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Связь CategoryAttribute -> ProductAttribute
        modelBuilder.Entity<ProductAttribute>()
            .HasOne(pa => pa.CategoryAttribute)
            .WithMany()
            .HasForeignKey(pa => pa.CategoryAttributeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product and ProductImages
        modelBuilder.Entity<Product>()
            .HasMany(p => p.ProductImages)
            .WithOne(pi => pi.Product)
            .HasForeignKey(p => p.ProductId);

        // Seller Relationships
        modelBuilder.Entity<Seller>()
            .HasMany(s => s.Marketplaces)
            .WithOne(m => m.Seller)
            .HasForeignKey(m => m.SellerId);

        modelBuilder.Entity<Marketplace>()
            .HasMany(m => m.Products)
            .WithOne(p => p.Marketplace)
            .HasForeignKey(p => p.MarketplaceId);

        base.OnModelCreating(modelBuilder);
    }
}
