namespace NationEase.Models;

public class State
{
    public int StateId { get; set; }

    public DateOnly Date { get; set; }

    public string Acronym { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public ICollection<City> Cities { get; set; } = new List<City>();

/*
    public string Print()
    {
        return $"#{StateId} | {Date:yyyy-MM-dd} | [{Acronym}] | {Name}";
    }
*/
}