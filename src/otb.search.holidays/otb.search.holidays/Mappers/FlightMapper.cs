using otb.search.holidays.Dtos;
using otb.search.holidays.Entities;

namespace otb.search.holidays.Mappers
{
    public static class FlightMapper
    {
        public static FlightResultDto ToDto(this FlightEntity flight)
        {
            return new FlightResultDto
            {
                Id = flight.Id,
                DepartingFrom = flight.From,
                TravellingTo = flight.To,
                Price = flight.Price
            };
        }
    }
}
