namespace otb.search.holidays.Dtos
{
    public class HolidaySearchResultDto
    {
        public int TotalPrice => Flight.Price + Hotel.Price;
        public FlightResultDto Flight { get; set; }
        public HotelResultDto Hotel { get; set; }
    }
}
