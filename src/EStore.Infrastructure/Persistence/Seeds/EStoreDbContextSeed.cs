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
        var laptopCategory = Category.Create("Laptop", null).Value;
        var smartphoneCategory = Category.Create("Smartphone", null).Value;

        var laptopGaming = Category.Create("Laptop Gaming", laptopCategory.Id).Value;
        var laptopForStudents = Category.Create("Laptop for Students", laptopCategory.Id).Value;
        var laptopOffice = Category.Create("Laptop Office", laptopCategory.Id).Value;
        var laptopProgramming = Category.Create("Laptop Programming", laptopCategory.Id).Value;

        var iphones = Category.Create("iPhones", smartphoneCategory.Id).Value;
        var samsungGalaxy = Category.Create("Samsung Galaxy", smartphoneCategory.Id).Value;

        return new List<Category>
        {
            laptopCategory,
            smartphoneCategory,
            laptopGaming,
            laptopForStudents,
            laptopOffice,
            laptopProgramming,
            iphones,
            samsungGalaxy
        };
    }

    public static IEnumerable<Brand> GetPreConfiguredBrands()
    {
        return new List<Brand>()
        {
            Brand.Create("Apple").Value,
            Brand.Create("Samsung").Value,
            Brand.Create("Lenovo").Value,
            Brand.Create("Dell").Value,
            Brand.Create("Asus").Value,
            Brand.Create("Acer").Value,
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
