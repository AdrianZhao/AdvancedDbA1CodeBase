namespace WebApplication2.Models
{
    public class StoreProvince
    {
        public string Province { get; set; }
        public HashSet<StoreLocation> Stores { get; set; }
    }
}
