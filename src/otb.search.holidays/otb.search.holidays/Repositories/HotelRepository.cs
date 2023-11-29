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
            throw new NotImplementedException();
        }
    }
}
