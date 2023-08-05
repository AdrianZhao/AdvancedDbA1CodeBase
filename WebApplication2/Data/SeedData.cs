using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
namespace WebApplication2.Data
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            RefactorContext db = new RefactorContext(serviceProvider.GetRequiredService<DbContextOptions<RefactorContext>>());
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            Brand dell = new Brand { Name = "Dell"};
            Brand alienware = new Brand { Name = "Alienware" };
            Brand asus = new Brand { Name = "Asus" };
            if (!db.Brands.Any())
            {
                db.Brands.Add(dell);
                db.Brands.Add(alienware);
                db.Brands.Add(asus);
                db.SaveChanges();
            }
            Laptop XPS13 = new Laptop { Model = "XPS13", Price = 1500, Condition = LaptopCondition.New, Brand = dell };
            Laptop X17 = new Laptop { Model = "X17", Price = 3000, Condition = LaptopCondition.Refurbished, Brand = alienware };
            Laptop G16 = new Laptop { Model = "G16", Price = 300, Condition = LaptopCondition.Rental, Brand = asus };
            if (!db.Laptops.Any())
            {
                db.Laptops.Add(XPS13);
                db.Laptops.Add(X17);
                db.Laptops.Add(G16);
                db.SaveChanges();
            }
            StoreLocation storeLocationOne = new StoreLocation { StreetName = "111 Street", Province = "MB" };
            StoreLocation storeLocationTwo = new StoreLocation { StreetName = "222 Blvd", Province = "AB" };
            StoreLocation storeLocationThree = new StoreLocation { StreetName = "333 Ave", Province = "BC" };
            if (!db.StoreLocations.Any())
            {
                db.StoreLocations.Add(storeLocationOne);
                db.StoreLocations.Add(storeLocationTwo);
                db.StoreLocations.Add(storeLocationThree);
                db.SaveChanges();
            }
            LaptopQuantity laptopQuantityOne = new LaptopQuantity { LaptopNumber = XPS13.Number, StoreLocationNumber = storeLocationOne.Number, Quantity = 5 };
            LaptopQuantity laptopQuantityTwo = new LaptopQuantity { LaptopNumber = X17.Number, StoreLocationNumber = storeLocationOne.Number, Quantity = 25 };
            LaptopQuantity laptopQuantityThree = new LaptopQuantity { LaptopNumber = G16.Number, StoreLocationNumber = storeLocationOne.Number, Quantity = -1 };
            LaptopQuantity laptopQuantityFour = new LaptopQuantity { LaptopNumber = XPS13.Number, StoreLocationNumber = storeLocationTwo.Number, Quantity = 15 };
            LaptopQuantity laptopQuantityFive = new LaptopQuantity { LaptopNumber = X17.Number, StoreLocationNumber = storeLocationTwo.Number, Quantity = 15 };
            LaptopQuantity laptopQuantitySix = new LaptopQuantity { LaptopNumber = G16.Number, StoreLocationNumber = storeLocationTwo.Number, Quantity = -1 };
            LaptopQuantity laptopQuantitySeven = new LaptopQuantity { LaptopNumber = XPS13.Number, StoreLocationNumber = storeLocationThree.Number, Quantity = 25 };
            LaptopQuantity laptopQuantityEight = new LaptopQuantity { LaptopNumber = X17.Number, StoreLocationNumber = storeLocationThree.Number, Quantity = 5 };
            LaptopQuantity laptopQuantityNine = new LaptopQuantity { LaptopNumber = G16.Number, StoreLocationNumber = storeLocationThree.Number, Quantity = -1 };
            if (!db.LaptopQuantities.Any())
            {
                db.LaptopQuantities.Add(laptopQuantityOne);
                db.LaptopQuantities.Add(laptopQuantityTwo);
                db.LaptopQuantities.Add(laptopQuantityThree);
                db.LaptopQuantities.Add(laptopQuantityFour);
                db.LaptopQuantities.Add(laptopQuantityFive);
                db.LaptopQuantities.Add(laptopQuantitySix);
                db.LaptopQuantities.Add(laptopQuantitySeven);
                db.LaptopQuantities.Add(laptopQuantityEight);
                db.LaptopQuantities.Add(laptopQuantityNine);
                db.SaveChanges();
            }
            await db.SaveChangesAsync();
        }
    }
}
