using AuchanTest.DTOs;
using AuchanTest.Entities;
using CsvHelper;
using System.Globalization;

namespace AuchanTest.Repositories
{
    public class CarsRepository
    {
        private readonly IEnumerable<Car> _cars;

        public CarsRepository(IEnumerable<Car> cars)
        {
            this._cars = cars;
        }

        public static CarsRepository Create()
        {
            var records = new List<Car>();

            using (var reader = new StreamReader("car_price_dataset.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Car>().ToList();               
            }

            return new CarsRepository(records);
        }

        public SearchResult GetCars(SearchCriteria search, SortCriteria sort, int page, int pageSize)
        {
            var query = _cars;

            if (!string.IsNullOrWhiteSpace(search.Brand))
            {
                query = query.Where(x => x.Brand.Contains(search.Brand, StringComparison.InvariantCultureIgnoreCase));
            }

            if (search.MinYear != 0)
            {
                query = query.Where(x => x.Year >= search.MinYear);
            }

            if (search.MaxMileage != 0)
            {
                query = query.Where(x => x.Mileage <= search.MaxMileage);
            }

            if (search.FuelType!= null)
            {
                query = query.Where(x => x.Fuel_Type == search.FuelType);
            }

            if (search.MinPrice != 0)
            {
                query = query.Where(x => x.Price >= search.MinPrice);
            }

            if (search.MaxPrice != 0)
            {
                query = query.Where(x => x.Price <= search.MaxPrice);
            }

            if (sort != null)
            {
                //a dictionary to map the sort column to the corresponding property func
                var orderFuncDictionary = new Dictionary<SortColumn, Func<Car, object>>
                {
                    {SortColumn.year, x => x.Year},
                    {SortColumn.mileage, x => x.Mileage},
                    {SortColumn.price, x => x.Price}
                };

                if (sort.Ascending)
                {
                    query = query.OrderBy(orderFuncDictionary[sort.Column]);
                }
                else
                {
                    query = query.OrderByDescending(orderFuncDictionary[sort.Column]);
                }
            }

            var aggregations = new ResultStatistics();

            if (query.Any())
            {
                aggregations.AveragePrice = Convert.ToInt32(query.Average(x => x.Price));
                aggregations.MostCommonFuelType = query.GroupBy(x => x.Fuel_Type)
                    .OrderByDescending(x => x.Count())
                    .Select(x => x.Key)
                    .FirstOrDefault().ToString();
                aggregations.NewestCarYear = query.Max(x => x.Year);
            };

            var pageResults = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new SearchResult
            {
                Results = pageResults,
                Aggregations = aggregations,
                Pagination = new PaginationResult
                {
                    Page = page,
                    PageSize = pageResults.Count(),
                    TotalCount = query.Count()
                }
            };
        }
    }
}
