using otb.search.holidays.Dtos;
using otb.search.holidays.Entities;

namespace otb.search.holidays.Mappers;

public static class HotelMapper
{
    public static HotelResultDto ToDto(this HotelEntity hotel)
    {
        return new HotelResultDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Price = hotel.PricePerNight * hotel.Nights
        };
    }
}