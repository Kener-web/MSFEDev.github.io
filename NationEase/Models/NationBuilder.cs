using System.Collections.ObjectModel;

namespace NationEase.Models;

public class NationBuilder
{
    public int CityCount => Cities.Count;
    public decimal AveragePopulation => CityCount == 0 ? 0m : Cities.Average(c => c.Population);
    public City? LargestCity => Cities.MaxBy(c => c.Population);
    public City? SmallestCity => Cities.MinBy(c => c.Population);
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