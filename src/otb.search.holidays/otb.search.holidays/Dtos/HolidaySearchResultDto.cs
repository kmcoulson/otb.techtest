﻿namespace otb.search.holidays.Dtos
{
    public class HolidaySearchResultDto
    {
        public int TotalPrice => Flight.Price + Hotel.Price;
        public FlightResultDto Flight { get; set; } = new();
        public HotelResultDto Hotel { get; set; } = new();
    }
}
