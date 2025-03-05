using AuchanTest.Entities;

namespace AuchanTest.DTOs
{
    public class SearchResult
    {
       public IEnumerable<Car> Results { get; set; }
       public ResultStatistics Aggregations { get; set; }
       public PaginationResult Pagination { get; set; }
    }
}
