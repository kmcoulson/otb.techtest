using System.Text.RegularExpressions;
using otb.search.holidays.Dtos;
using otb.search.holidays.Entities;
using otb.search.holidays.Mappers;
using otb.search.holidays.Repositories;

namespace otb.search.holidays.Services
{
    public interface IHolidaySearchService
    {
        Task<IEnumerable<HolidaySearchResultDto>> SearchHolidays(string from, string to, DateOnly date, int nights);
    }

    public class HolidaySearchService : IHolidaySearchService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IAirportRepository _airportRepository;

        public HolidaySearchService(IFlightRepository flightRepository, IHotelRepository hotelRepository, IAirportRepository airportRepository)
        {
            _flightRepository = flightRepository;
            _hotelRepository = hotelRepository;
            _airportRepository = airportRepository;
        }

        public async Task<IEnumerable<HolidaySearchResultDto>> SearchHolidays(string from, string to, DateOnly date, int nights)
        {
            var getFromCodesTask = Task.Run(() => GetAirportCodes(from));
            var getToCodesTask = Task.Run(() => GetAirportCodes(to));

            await Task.WhenAll(getFromCodesTask, getToCodesTask);

            var fromCodes = getFromCodesTask.Result;
            var toCodes = getToCodesTask.Result;

            var flightSearchTask = Task.Run(() => GetMatchingFlights(fromCodes, toCodes, date));
            var hotelSearchTask = Task.Run(() => GetMatchingHotels(toCodes, date, nights));

            await Task.WhenAll(flightSearchTask, hotelSearchTask);

            var flightResults = flightSearchTask.Result.ToList();
            var hotelResults = hotelSearchTask.Result.ToList();

            var results = new List<HolidaySearchResultDto>();

            foreach (var flight in flightResults)
            {
                foreach (var hotel in hotelResults)
                {
                    results.Add(new HolidaySearchResultDto
                    {
                        Flight = flight.ToDto(),
                        Hotel = hotel.ToDto()
                    });
                }
            }

            return results.OrderBy(x => x.TotalPrice);
        }

        private async Task<List<string>> GetAirportCodes(string destination)
        {
            var fromCodes = new List<string>();

            if (destination.StartsWith("ANY ", StringComparison.CurrentCultureIgnoreCase))
            {
                var anyCityRegexPattern = @"(?i)\b(?:Any|Airport)\b";
                var cityName = Regex.Replace(destination, anyCityRegexPattern, "").Trim();
                var airportCodes = await _airportRepository.GetAirportCodesForCity(cityName);
                fromCodes.AddRange(airportCodes);
            }
            else if (destination.Equals("ANY", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                fromCodes.Add(destination);
            }

            return fromCodes;
        }

        private async Task<IEnumerable<FlightEntity>> GetMatchingFlights(List<string> fromCodes, List<string> toCodes, DateOnly date)
        {
            var allFlights = await _flightRepository.GetAll();

            var validFlights = allFlights
                .Where(x =>
                    (!fromCodes.Any() || fromCodes.Any(f => f.Equals(x.From, StringComparison.OrdinalIgnoreCase)))
                    && (!toCodes.Any() || toCodes.Any(t => t.Equals(x.To, StringComparison.OrdinalIgnoreCase)))
                    && x.DepartureDate.Equals(date))
                .ToList();

            return validFlights;
        }

        private async Task<IEnumerable<HotelEntity>> GetMatchingHotels(List<string> toCodes, DateOnly date, int nights)
        {
            var allHotels = await _hotelRepository.GetAll();

            var validHotels = allHotels
                .Where(x =>
                    (!toCodes.Any() || x.LocalAirports.Intersect(toCodes, StringComparer.CurrentCultureIgnoreCase).Any())
                    && x.ArrivalDate.Equals(date)
                    && x.Nights == nights)
                .ToList();
            return validHotels;
        }
    }
}
