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
        private readonly CarsRepository _carsRepository;

        public CarsController(CarsRepository carsRepository)
        {
            this._carsRepository = carsRepository;
        }

        [HttpGet("search")]
        public IActionResult GetCars([FromQuery] SearchCriteria search, [FromQuery] string sortBy, [FromQuery] int page, [FromQuery] int pageSize)
        {

            if (page == 0 || pageSize == 0)
            {
                return BadRequest("Page and PageSize must be valid");
            }

            if (search.MinPrice > search.MaxPrice)
            {
                return BadRequest("Invalid price range");
            }

            var sortParts = sortBy.Split('_');

            SortCriteria sort = null;

            //in case of invalid sort column, there will be no sorting
            if (Enum.TryParse<SortColumn>(sortParts[0], out var column))
            {
                sort = new SortCriteria { 
                    Column = column, 
                    Ascending = sortParts.Length ==1 || sortParts[1].ToLower() != "desc" 
                };
            }

            var result = _carsRepository.GetCars(search, sort, page, pageSize);

            return Ok(result);
        }

    }

}
