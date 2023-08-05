using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using WebApplication2.Data;
using WebApplication2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("RefactorContextConnection");
builder.Services.AddDbContext<RefactorContext>(options =>
{
    options.UseSqlServer(connectionString);
});
WebApplication app = builder.Build();
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}
app.MapGet("/laptops/search", (RefactorContext db, decimal? priceAbove, decimal? priceBelow, int? brandId, Guid? storeNumber, string? province, string? LaptopCondition, string? searchPhrase) =>
{
    try
    {
        HashSet<Laptop> result = db.Laptops.Include(l => l.LaptopQuantities).ThenInclude(lq => lq.StoreLocation).Include(l => l.Brand).ToHashSet();
        if (priceAbove != null && priceAbove <= result.Max(l => l.Price))
        {
            result = result.Where(l => l.Price >= priceAbove).ToHashSet();
        } 
        else if (priceAbove > result.Max(l => l.Price))
        {
            throw new ArgumentOutOfRangeException($"The amount you entered '{priceAbove}' is higher than the price of all laptops.");
        }
        if (priceBelow != null && priceBelow >= result.Min(l => l.Price))
        {
            result = result.Where(l => l.Price <= priceBelow).ToHashSet();
        }
        else if (priceBelow < result.Min(l => l.Price))
        {
            throw new ArgumentOutOfRangeException($"The amount you entered '{priceBelow}' is lower than the price of all laptops.");
        }
        if (brandId != null)
        {
            if (db.Brands.Any(b => b.Id == brandId))
            {
                result = result.Where(l => l.BrandId == brandId).ToHashSet();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The brand ID ({brandId}) you entered is not in the database.");
            }
        }
        if (storeNumber != null)
        {
            if (db.StoreLocations.Any(sl => sl.Number == storeNumber))
            {
                result = result.Where(l => l.LaptopQuantities.Any(sl => sl.StoreLocationNumber == storeNumber && sl.Quantity > 0)).ToHashSet();
                if (result == null)
                {
                    throw new Exception($"The store with ID ({storeNumber}) do not have any laptops in stock.");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The store with ID ({storeNumber}) do not exist.");
            }
        }
        else if (!string.IsNullOrEmpty(province))
        {
            if (db.StoreLocations.Any(sl => sl.Province == province))
            {
                result = result.Where(l => l.LaptopQuantities.Any(sl => sl.StoreLocation.Province == province && sl.Quantity > 0)).ToHashSet();
                if (result == null)
                {
                    throw new Exception($"The store in province ({province}) do not have any laptops in stock.");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The province ({province}) do not exist.");
            }
        }
        if (!string.IsNullOrEmpty(LaptopCondition))
        {
            HashSet<string> allConditions = new HashSet<string>();
            // https://learn.microsoft.com/en-us/dotnet/api/system.enum.getnames?view=net-7.0
            foreach (string conditionName in Enum.GetNames(typeof(LaptopCondition))){
                allConditions.Add(conditionName.ToLower());
            }
            string laptopConditionLower = LaptopCondition.ToLower();
            if (allConditions.Contains(laptopConditionLower))
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.enum.parse?view=net-7.0
                LaptopCondition conditionEnum = Enum.Parse<LaptopCondition>(LaptopCondition, true);
                result = result.Where(l => l.Condition == conditionEnum).ToHashSet();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The laptop condition '{LaptopCondition}' is not exist.");
            }
        }
        if (!string.IsNullOrEmpty(searchPhrase))
        {
            if (db.Laptops.Any(l => l.Model.ToLower().Contains(searchPhrase.ToLower())))
            {
                result = result.Where(l => l.Model.ToLower().Contains(searchPhrase.ToLower())).ToHashSet();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"The Search Phrase ({searchPhrase}) you entered do not match any laptop.");
            }
        }
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/stores/{storeNumber}/inventory", (RefactorContext db, Guid storeNumber) =>
{
    try
    {
        HashSet<Laptop> laptopsInventory = db.LaptopQuantities.Where(lq => lq.StoreLocationNumber == storeNumber && lq.Quantity > 0).Select(l => l.Laptop).ToHashSet();
        return Results.Ok(laptopsInventory);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/stores/{storeNumber}/{laptopNumber}/changeQuantity", (RefactorContext db, Guid storeNumber, Guid laptopNumber, int amount) =>
{
    try
    {
        Laptop? laptop = db.Laptops.FirstOrDefault(l => l.Number == laptopNumber);
        WebApplication2.Models.StoreLocation? storeLocation = db.StoreLocations.FirstOrDefault(sl => sl.Number == storeNumber);
        if (laptop != null && storeLocation != null)
        {
            LaptopQuantity? laptopQuantity = db.LaptopQuantities.FirstOrDefault(lq => lq.LaptopNumber == laptopNumber && lq.StoreLocationNumber == storeNumber);
            if (laptopQuantity != null)
            {
                int oldQuantity = laptopQuantity.Quantity;
                laptopQuantity.Quantity += amount;
                db.SaveChanges();
                return Results.Ok($"Laptop {laptop.Model} increased quantity from {oldQuantity} to {laptopQuantity.Quantity}.");
            }
            else
            {
                return Results.NotFound($"Laptop {laptopNumber} at store {storeNumber} not found.");
            }
        }
        else if (laptop != null && storeLocation == null)
        {
            return Results.NotFound("Store not found.");
        }
        else if (laptop == null && storeLocation != null)
        {
            return Results.NotFound("Laptop not found.");
        }
        else
        {
            return Results.NotFound("Laptop and store are all not found.");
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/brands/{brandId}/averagePrice", (RefactorContext db, int brandId) =>
{
    try
    {
        Brand? brand = db.Brands.FirstOrDefault(b => b.Id == brandId);
        if (brand != null)
        {
            HashSet<Laptop> resultLaptop = db.Laptops.Where(l => l.BrandId == brandId).ToHashSet();
            if (resultLaptop.Count > 0)
            {
                int count = resultLaptop.Count;
                decimal total = resultLaptop.Sum(l => l.Price);
                decimal average = total / count;
                return Results.Ok(new
                {
                    LaptopCount = count,
                    AveragePrice = average
                });
            }
            else
            {
                return Results.NotFound($"No laptops found for brand with ID '{brandId}'.");
            }
        }
        else
        {
            return Results.NotFound($"Brand with ID '{brandId}' not found.");
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/stores/byProvince", (RefactorContext db) =>
{
    try
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.groupby?view=net-6.0
        /*
        HashSet<StoreProvince> storesByProvince = db.StoreLocations
                .GroupBy(sl => sl.Province)
                .Where(sl => sl.Any())
                .Select(sbp => new StoreProvince
                {
                    Province = sbp.Key,
                    Stores = new HashSet<WebApplication2.Models.StoreLocation>(sbp
                        .Select(s => new WebApplication2.Models.StoreLocation
                        {
                            Province = s.Province,
                            Number = s.Number,
                            StreetName = s.StreetName
                        }))
                }).ToHashSet();
        */
        
        var storesByProvince = db.StoreLocations
            .GroupBy(s => s.Province)
            .Where(group => group.Any())
            .Select(group => new
            {
                Province = group.Key,
                Stores = new HashSet<WebApplication2.Models.StoreLocation>(group.Select(s => new WebApplication2.Models.StoreLocation
                {
                    Province = s.Province,
                    Number = s.Number,
                    StreetName = s.StreetName
                }))
            }).ToList(); 
        if (storesByProvince != null)
        {
            return Results.Ok(storesByProvince);
        }
        else
        {
            return Results.NotFound("No provinces with stores found.");
        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.Run();