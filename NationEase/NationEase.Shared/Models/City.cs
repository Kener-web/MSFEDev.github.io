namespace NationEase.Models;

public class City
{
    public int CityId { get; set; }

    public int StateId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Population { get; set; }

    public DateOnly? FoundingDate { get; set; }

    public State? State { get; set; }

/*
    public string Print()
    {
        return $"[{State?.Acronym}] {Name} : {Population:N0}";
    }
*/
}