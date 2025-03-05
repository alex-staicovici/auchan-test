using AuchanTest.DTOs;
using AuchanTest.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace AuchanTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        [HttpGet("search")]
        public IActionResult GetCars([FromQuery] SearchCriteria search, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int pageSize)
        {

            if (page == 0 || pageSize == 0)
            {
                //TODO : validation
            }

            var sortParts = sortBy.Split('_');

            SortCriteria sort = null;

            if (Enum.TryParse<SortColumn>(sortParts[0], out var column))
            {
                sort = new SortCriteria { 
                    Column = column, 
                    Ascending = sortParts.Length ==1 || sortParts[1].ToLower() != "desc" 
                };
            }            

            var cars = CarsRepository.Create().GetCars(search, sort);

            var aggregations = new ResultStatistics();

            if(cars.Any())
            {
                aggregations.AveragePrice = Convert.ToInt32(cars.Average(x => x.Price));
                aggregations.MostCommonFuelType = cars.GroupBy(x => x.Fuel_Type)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .FirstOrDefault().ToString();
                aggregations.NewestCarYear = cars.Max(x => x.Year);
            };

            return Ok(new SearchResult{
                Results = cars.Skip((page-1)* pageSize).Take(pageSize),
                Aggregations = aggregations,
                Pagination = new PaginationResult
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = cars.Count()
                }
            });
        }

    }

}
