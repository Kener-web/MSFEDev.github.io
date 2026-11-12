namespace NationEase.Models;

public class NationCity
{
    public int NationCityId { get; set; }

    public int CityId { get; set; }

    public City? City { get; set; }
}