using System.Text.Json.Serialization;

namespace WebApplication2.Models
{
    public class LaptopQuantity
    {
        public Guid LaptopNumber { get; set; }
        // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties?pivots=dotnet-6-0
        [JsonIgnore]
        public Laptop Laptop { get; set; }
        public Guid StoreLocationNumber { get; set; }
        [JsonIgnore]
        public StoreLocation StoreLocation { get; set; }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentException(nameof(value));
                }
                _quantity = value;
            }
        }
    }
}
