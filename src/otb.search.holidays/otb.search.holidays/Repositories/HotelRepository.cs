using Newtonsoft.Json;
using otb.search.holidays.Entities;

namespace otb.search.holidays.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<HotelEntity>> GetAll();
    }


    public class HotelRepository : IHotelRepository
    {
        public Task<IEnumerable<HotelEntity>> GetAll()
        {
            var json = File.ReadAllText("Data/hotels.json");
            var data = JsonConvert.DeserializeObject<IEnumerable<HotelEntity>>(json);

            if (data == null) throw new Exception("An error occurred parsing data for Hotels");

            return Task.FromResult(data);
        }
    }
}
