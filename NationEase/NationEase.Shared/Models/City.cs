namespace NationEase.Models;

public class City
{
    public int CityId { get; set; }

    public int StateId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Population { get; set; }

    public DateOnly? FoundingDate { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public State? State { get; set; }
}