using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.BrandAggregate;
using EStore.Domain.CategoryAggregate;
using EStore.Domain.DiscountAggregate;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Persistence.Seeds;

public static class EStoreDbContextSeed
{
    public static async Task SeedAsync(EStoreDbContext context, IUnitOfWork unitOfWork)
    {
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }

        if (!context.Categories.Any())
        {
            var categories = GetPreConfiguredCategories();

            await context.Categories.AddRangeAsync(categories);
        }

        if (!context.Brands.Any())
        {
            var brands = GetPreConfiguredBrands();

            await context.Brands.AddRangeAsync(brands);
        }

        if (!context.Discounts.Any())
        {
            var discounts = GetPreConfiguredDiscounts();

            await context.Discounts.AddRangeAsync(discounts);
        }

        await unitOfWork.SaveChangesAsync();
    }

    public static IEnumerable<Category> GetPreConfiguredCategories()
    {
        var laptopCategory = Category.Create("Laptop", "laptop", null, null).Value;
        var smartphoneCategory = Category.Create("Smartphone", "smartphone", null, null).Value;

        var laptopAsus = Category.Create("Laptop Asus", "laptop-asus", null, laptopCategory.Id).Value;
        var laptopDell = Category.Create("Laptop Dell", "laptop-dell", null, laptopCategory.Id).Value;
        var laptopLenovo = Category.Create("Laptop Lenovo", "laptop-lenovo", null, laptopCategory.Id).Value;
        var laptopAcer = Category.Create("Laptop Acer", "laptop-acer", null, laptopCategory.Id).Value;

        var iphones = Category.Create("iPhones", "iphones", null, smartphoneCategory.Id).Value;
        var samsungGalaxy = Category.Create("Samsung Galaxy", "samsung-galaxy", null, smartphoneCategory.Id).Value;

        return new List<Category>
        {
            laptopCategory,
            smartphoneCategory,
            laptopAsus,
            laptopDell,
            laptopLenovo,
            laptopAcer,
            iphones,
            samsungGalaxy
        };
    }

    public static IEnumerable<Brand> GetPreConfiguredBrands()
    {
        return new List<Brand>()
        {
            Brand.Create("Apple", string.Empty).Value,
            Brand.Create("Samsung", string.Empty).Value,
            Brand.Create("Lenovo", string.Empty).Value,
            Brand.Create("Dell", string.Empty).Value,
            Brand.Create("Asus", string.Empty).Value,
            Brand.Create("Acer", string.Empty).Value,
        };
    }

    public static IEnumerable<Discount> GetPreConfiguredDiscounts()
    {
        var startDate = DateTime.UtcNow.AddMinutes(10);
        var endDate = DateTime.UtcNow.AddYears(1);

        return new List<Discount>()
        {
            Discount.Create("Black Friday", true, 0.2m, 0, startDate, endDate).Value,
            Discount.Create("Christmas", true, 0.1m, 0, startDate, endDate).Value,
            Discount.Create("New Year", true, 0.15m, 0, startDate, endDate).Value,
        };
    }
}
