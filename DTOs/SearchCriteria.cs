using AuchanTest.Entities;

namespace AuchanTest.DTOs
{
    public class SearchCriteria
    {
        public string Brand { get; set; }
        public int MinYear { get; set; }
        public int MaxMileage { get; set; }
        public FuelType? FuelType { get; set; }
        public int MaxPrice { get; set; }
        public int MinPrice { get; set; }
    }
}
