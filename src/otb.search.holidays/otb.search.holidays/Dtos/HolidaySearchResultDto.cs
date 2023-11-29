using otb.search.holidays.Entities;

namespace otb.search.holidays.Dtos
{
    public class HolidaySearchResultDto
    {
        public FlightEntity Flight { get; set; }
        public HotelEntity Hotel { get; set; }
    }
}
