using otb.search.holidays.Entities;
using otb.search.holidays.Enums;

namespace otb.search.holidays.Repositories
{
    public interface IHotelRepository
    {
        Task<IEnumerable<HotelEntity>> GetAll();
    }


    public class HotelRepository : BaseDataRepository, IHotelRepository
    {
        public Task<IEnumerable<HotelEntity>> GetAll()
        {
            var data = GetData<HotelEntity>(DataType.Hotels);
            return Task.FromResult(data);
        }
    }
}
