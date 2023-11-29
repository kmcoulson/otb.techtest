using Newtonsoft.Json;
using otb.search.holidays.Enums;

namespace otb.search.holidays.Repositories
{
    public class BaseDataRepository
    {
        protected IEnumerable<T> GetData<T>(DataType type)
        {
            var dataPath = $"Data/{type}.json";
            var json = File.ReadAllText(dataPath);
            var data = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            if (data == null) throw new Exception($"An error occurred parsing data for {type}");

            return data;
        }
    }
}
