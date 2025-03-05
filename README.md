#This is a basic n-tier API with one controller and one repository.
- The repo class acts as a factory for the repo instance.
- There is only one instance (singleton) of the repo since the datasource is the same and should only be loaded in memory once (at startup).
- No interfaces were used for dependency injection in this project due to low complexity
- No domain logic/business logic/service layer due to low complexity
- If the user inputs anything other then 'desc' for sorting then by default it is 'asc' (no validation on this part)
- There is an enum for FuelType since it has to be an exact match for the search criteria and it's easier to map to an enum. There is no enum for other fields, like Transmission, since the requirements did not specify any other such constraints.

  #Examples of api calls I've used using chrome:
  
/api/cars/search?brand=ford&minYear=2023&maxPrice=95000&minPrice=10000&sortBy=year_asc&page=2&pageSize=10&fuelType=Electric

/api/cars/search?brand=ford&minYear=2023&maxPrice=95000&minPrice=10000&sortBy=year_asc&page=2&pageSize=10

/api/cars/search?brand=ford&minYear=2023&maxPrice=95000&minPrice=10000&sortBy=year_desc&page=2&pageSize=10

/api/cars/search?brand=ford&minYear=2023&maxPrice=95000&sortBy=year_desc&page=2&pageSize=10

/api/cars/search?brand=ford&minYear=2018&maxPrice=25000&sortBy=price_desc&page=1&pageSize=10

/api/cars/search?brand=volk&minYear=2018&maxPrice=25000&sortBy=year_desc&page=1&pageSize=50
