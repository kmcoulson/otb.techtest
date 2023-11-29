using System.Net.Http.Json;
using otb.search.holidays.Http;

namespace otb.search.holidays.Repositories
{
    public interface IAirportRepository
    {
        Task<IEnumerable<string>> GetAirportCodesForCity(string city);
    }

    public class AirportRepository : IAirportRepository
    {
        public async Task<IEnumerable<string>> GetAirportCodesForCity(string city)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.AirportApi.Url);
            client.DefaultRequestHeaders.Add("APC-Auth", Constants.AirportApi.Key);
            client.DefaultRequestHeaders.Add("APC-Auth-Secret", Constants.AirportApi.Secret);

            var result = await client.GetFromJsonAsync<AirportsDataResponse>($"{Constants.AirportApi.Endpoint}{city}");
            return result?.Airports.Select(x => x.Iata) ?? new List<string>();
        }
    }
}
