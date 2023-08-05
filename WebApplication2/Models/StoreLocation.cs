using System;

namespace WebApplication2.Models
{
    public class StoreLocation
    {
        public Guid Number { get; set; }
        private string _streetName { get; set; }
        public string StreetName
        {
            get => _streetName;
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Street name must be at least three characters in length.");
                }
                _streetName = value;
            }
        }
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
