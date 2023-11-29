namespace otb.search.holidays.Dtos;

public class FlightResultDto
{
    public int Id { get; set; }
    public string DepartingFrom { get; set; } = string.Empty;
    public string TravellingTo { get; set; } = string.Empty;
    public int Price { get; set; }
}