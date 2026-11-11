using System.Collections.ObjectModel;

namespace NationEase.Models;

public class NationBuilder
{
    public int CityCount => Cities.Count;
    public decimal AveragePopulation => Cities.Any() ? Cities.Average(c => c.Population) : 0m;
    public City? LargestCity => Cities.OrderByDescending(c => c.Population).FirstOrDefault();
    public City? SmallestCity => Cities.OrderBy(c => c.Population).FirstOrDefault();
    public ObservableCollection<City> Cities { get; set; } = new();

    public decimal TotalPopulation =>
        Cities.Sum(c => c.Population);

    public void AddCity(City city)
    {
        if (!Cities.Any(c => c.CityId == city.CityId))
        {
            Cities.Add(city);
        }
    }

    public void RemoveCity(int cityId)
    {
        var city = Cities.FirstOrDefault(c => c.CityId == cityId);

        if (city != null)
        {
            Cities.Remove(city);
        }
    }

    public void Clear()
    {
        Cities.Clear();
    }
}