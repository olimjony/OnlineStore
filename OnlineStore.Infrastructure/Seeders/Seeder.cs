using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities;
using OnlineStore.Infrastructure.Persistence;

namespace OnlineStore.Infrastructure.Seeders;

internal class Seeder(DataContext _dataContext) : ISeeder
{
    public async Task Seed( ){
        if(await _dataContext.Database.CanConnectAsync( )){
            if(!_dataContext.Roles.Any()){
                
                var roles = new List<Role>(){
                    new Role(){ Name = Roles.User },
                    new Role(){ Name = Roles.Seller },
                    new Role(){ Name = Roles.Admin }
                };

                _dataContext.Roles.AddRange(roles);
                await _dataContext.SaveChangesAsync();
            }
            if(!_dataContext.Categories.Any()){

                var categories = new Category{
                    Name = "Электроника",
                    CategoryAttributes = new List<CategoryAttribute> {
                        new CategoryAttribute { Name = "Вес", Type = "decimal" }
                    },
                    SubCategories = new List<Category>(){
                        new Category {
                            Name = "Смартфоны",
                            CategoryAttributes = new List<CategoryAttribute> {
                                new CategoryAttribute(){ Name = "Модель", Type = "string" },
                                new CategoryAttribute(){ Name = "Цвет", Type = "string" },
                                new CategoryAttribute(){ Name = "Система", Type = "string" },
                                new CategoryAttribute(){ Name = "Память", Type = "string" },
                                new CategoryAttribute(){ Name = "Оперативная память", Type = "string" },
                                new CategoryAttribute(){ Name = "Операционная система", Type = "string" },
                                new CategoryAttribute(){ Name = "Процессор", Type = "string" }
                            } 
                        },
                        new Category { Name = "Ноутбуки",
                            CategoryAttributes = new List<CategoryAttribute> {
                                new CategoryAttribute(){ Name = "Модель", Type = "string" },
                                new CategoryAttribute(){ Name = "Цвет", Type = "string" },
                                new CategoryAttribute(){ Name = "Система", Type = "string" },
                                new CategoryAttribute(){ Name = "Память", Type = "string" },
                                new CategoryAttribute(){ Name = "Оперативная память", Type = "string" },
                                new CategoryAttribute(){ Name = "Операционная система", Type = "string" },
                                new CategoryAttribute(){ Name = "Процессор", Type = "string" },
                                new CategoryAttribute(){ Name = "Видеокарта", Type = "string" }  
                            } },
                        new Category { Name = "Мониторы", 
                            CategoryAttributes = new List<CategoryAttribute> {
                                new CategoryAttribute(){ Name = "Модель", Type = "string" },
                                new CategoryAttribute(){ Name = "Цвет", Type = "string" },
                                new CategoryAttribute(){ Name = "Разрешение", Type = "string" },
                                new CategoryAttribute(){ Name = "Частота", Type = "string" },
                                new CategoryAttribute(){ Name = "Цветовая схема", Type = "string" }
                            },
                            SubCategories = new List<Category>(){
                                new Category {
                                    Name = "Умные Мониторы",
                                    CategoryAttributes = new List<CategoryAttribute>(){
                                        new CategoryAttribute(){ Name = "Операционная система", Type = "string" },
                                        new CategoryAttribute(){ Name = "Память", Type = "string" },
                                        new CategoryAttribute(){ Name = "Оперативная память", Type = "string" },
                                    }
                                }
                            }
                        },
                        new Category { Name = "Компьютеры"}                       
                    }  
                };

                _dataContext.Categories.AddRange(categories);
                await _dataContext.SaveChangesAsync();
                
                /*var electronics = new Category { Name = "Electronics" };
                electronics.CategoryAttributes.Add(new CategoryAttribute { Name = "Brand" });
                electronics.CategoryAttributes.Add(new CategoryAttribute { Name = "Model" });

                var laptops = new Category { Name = "Laptops", CategoryAttributes = new List<CategoryAttribute> {
                    new CategoryAttribute { Name = "Processor" },
                    new CategoryAttribute { Name = "RAM" },
                    new CategoryAttribute { Name = "Storage" }
                }};
        
                var smartphones = new Category { Name = "Smartphones", CategoryAttributes = new List<CategoryAttribute> {
                    new CategoryAttribute { Name = "Screen Size" },
                    new CategoryAttribute { Name = "Battery Capacity" },
                    new CategoryAttribute { Name = "Camera Resolution" }
                }};
                electronics.SubCategories.Add(laptops);

                var clothing = new Category { Name = "Clothing" };
                clothing.CategoryAttributes.Add(new CategoryAttribute { Name = "Size" });
                clothing.CategoryAttributes.Add(new CategoryAttribute { Name = "Material" });

                var winterClothes = new Category { Name = "Winter Clothes", CategoryAttributes = new List<CategoryAttribute> {
                    new CategoryAttribute { Name = "Insulation Type" }
                }};
        
                var fallClothes = new Category { Name = "Fall Clothes", CategoryAttributes = new List<CategoryAttribute> {
                    new CategoryAttribute { Name = "Layering" }
                }};

                clothing.Products = new List<Product> { };*/

                await _dataContext.SaveChangesAsync();
            }
        }
    }

}