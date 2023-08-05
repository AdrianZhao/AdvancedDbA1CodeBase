using System;

namespace WebApplication2.Models
{
    public class StoreLocation
    {
        public Guid Number { get; set; }
        public string StreetName { get; set; }
        private string _province;
        public string Province { get { return _province; } set
            {
                if (!IsValid(value))
                {
                    throw new ArgumentException("Invalid province format.");
                }
                _province = value;
            }
        }
        private static bool IsValid(string value)
        {
            HashSet<string> ifValid = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT" };
            return ifValid.Contains(value);
        }
        public HashSet<LaptopQuantity> LaptopQuantities { get; set; } = new HashSet<LaptopQuantity>();
    }
}
