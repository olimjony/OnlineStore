﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnlineStore.Infrastructure.Persistence;

#nullable disable

namespace OnlineStore.Infrastructure.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241029115341_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OnlineStore.Domain.Entities.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CartIconURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.CartProduct", b =>
                {
                    b.Property<int>("CartId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Liked")
                        .HasColumnType("boolean");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("CartId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartProducts");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.CategoryAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("CategoryAttribute");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Marketplace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Approved")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SellerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("Marketplaces");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MarketplaceId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ProductIcon")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("StockQuantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("MarketplaceId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryAttributeId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryAttributeId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAttribute");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("boolean");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Seller", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MaxMarketplaces")
                        .HasColumnType("integer");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId")
                        .IsUnique();

                    b.ToTable("Sellers");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MaxCarts")
                        .HasColumnType("integer");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("UserProfileId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserProfileId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Cart", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.CartProduct", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Cart", "Cart")
                        .WithMany("CartProducts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.Domain.Entities.Product", "Product")
                        .WithMany("CartProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Category", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Category", "ParentCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.CategoryAttribute", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Category", "Category")
                        .WithMany("CategoryAttributes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Marketplace", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Seller", "Seller")
                        .WithMany("Marketplaces")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Order", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Product", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.Domain.Entities.Marketplace", "Marketplace")
                        .WithMany("Products")
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Marketplace");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.ProductAttribute", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.CategoryAttribute", "CategoryAttribute")
                        .WithMany()
                        .HasForeignKey("CategoryAttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.Domain.Entities.Product", "Product")
                        .WithMany("Attributes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryAttribute");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.ProductImage", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Seller", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.UserProfile", "UserProfile")
                        .WithOne("Seller")
                        .HasForeignKey("OnlineStore.Domain.Entities.Seller", "UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.User", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.UserProfile", "UserProfile")
                        .WithOne("User")
                        .HasForeignKey("OnlineStore.Domain.Entities.User", "UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("OnlineStore.Domain.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineStore.Domain.Entities.UserProfile", "UserProfile")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Cart", b =>
                {
                    b.Navigation("CartProducts");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Category", b =>
                {
                    b.Navigation("CategoryAttributes");

                    b.Navigation("Products");

                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Marketplace", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Product", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("CartProducts");

                    b.Navigation("Orders");

                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.Seller", b =>
                {
                    b.Navigation("Marketplaces");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("OnlineStore.Domain.Entities.UserProfile", b =>
                {
                    b.Navigation("Seller");

                    b.Navigation("User")
                        .IsRequired();

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}