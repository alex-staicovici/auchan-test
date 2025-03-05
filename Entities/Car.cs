﻿namespace AuchanTest.Entities
{
    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Engine_Size { get; set; }
        public FuelType Fuel_Type { get; set; }
        public string Transmission { get; set; }
        public int Mileage { get; set; }        
        public int Doors { get; set; }
        public int Owner_Count { get; set; }
        public int Price { get; set; }
    }
}
