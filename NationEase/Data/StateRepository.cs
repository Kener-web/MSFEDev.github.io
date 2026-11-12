using NationEase.Models;

namespace NationEase.Data;

public static class StateRepository
{
    public static List<State> GetStates()
    {
        return new()
        {
            new() { StateId = 1,  Date = new DateOnly(1787,12,07), Acronym = "DE", Name = "Delaware" },
            new() { StateId = 2,  Date = new DateOnly(1787,12,12), Acronym = "PA", Name = "Pennsylvania" },
            new() { StateId = 3,  Date = new DateOnly(1787,12,18), Acronym = "NJ", Name = "New Jersey" },
            new() { StateId = 4,  Date = new DateOnly(1788,01,02), Acronym = "GA", Name = "Georgia" },
            new() { StateId = 5,  Date = new DateOnly(1788,01,09), Acronym = "CT", Name = "Connecticut" },
            new() { StateId = 6,  Date = new DateOnly(1788,02,06), Acronym = "MA", Name = "Massachusetts" },
            new() { StateId = 7,  Date = new DateOnly(1788,04,28), Acronym = "MD", Name = "Maryland" },
            new() { StateId = 8,  Date = new DateOnly(1788,05,23), Acronym = "SC", Name = "South Carolina" },
            new() { StateId = 9,  Date = new DateOnly(1788,06,21), Acronym = "NH", Name = "New Hampshire" },
            new() { StateId = 10, Date = new DateOnly(1788,06,25), Acronym = "VA", Name = "Virginia" },
            new() { StateId = 11, Date = new DateOnly(1788,07,26), Acronym = "NY", Name = "New York" },
            new() { StateId = 12, Date = new DateOnly(1788,11,21), Acronym = "NC", Name = "North Carolina" },
            new() { StateId = 13, Date = new DateOnly(1790,05,29), Acronym = "RI", Name = "Rhode Island" },
            new() { StateId = 14, Date = new DateOnly(1791,03,04), Acronym = "VT", Name = "Vermont" },
            new() { StateId = 15, Date = new DateOnly(1792,06,01), Acronym = "KY", Name = "Kentucky" },
            new() { StateId = 16, Date = new DateOnly(1796,06,01), Acronym = "TN", Name = "Tennessee" },
            new() { StateId = 17, Date = new DateOnly(1803,03,01), Acronym = "OH", Name = "Ohio" },
            new() { StateId = 18, Date = new DateOnly(1812,04,30), Acronym = "LA", Name = "Louisiana" },
            new() { StateId = 19, Date = new DateOnly(1816,12,11), Acronym = "IN", Name = "Indiana" },
            new() { StateId = 20, Date = new DateOnly(1817,12,10), Acronym = "MS", Name = "Mississippi" },
            new() { StateId = 21, Date = new DateOnly(1818,12,03), Acronym = "IL", Name = "Illinois" },
            new() { StateId = 22, Date = new DateOnly(1819,12,14), Acronym = "AL", Name = "Alabama" },
            new() { StateId = 23, Date = new DateOnly(1820,03,15), Acronym = "ME", Name = "Maine" },
            new() { StateId = 24, Date = new DateOnly(1821,08,10), Acronym = "MO", Name = "Missouri" },
            new() { StateId = 25, Date = new DateOnly(1836,06,15), Acronym = "AR", Name = "Arkansas" },
            new() { StateId = 26, Date = new DateOnly(1837,01,26), Acronym = "MI", Name = "Michigan" },
            new() { StateId = 27, Date = new DateOnly(1845,03,03), Acronym = "FL", Name = "Florida" },
            new() { StateId = 28, Date = new DateOnly(1845,12,29), Acronym = "TX", Name = "Texas" },
            new() { StateId = 29, Date = new DateOnly(1846,12,28), Acronym = "IA", Name = "Iowa" },
            new() { StateId = 30, Date = new DateOnly(1848,05,29), Acronym = "WI", Name = "Wisconsin" },
            new() { StateId = 31, Date = new DateOnly(1850,09,09), Acronym = "CA", Name = "California" },
            new() { StateId = 32, Date = new DateOnly(1858,05,11), Acronym = "MN", Name = "Minnesota" },
            new() { StateId = 33, Date = new DateOnly(1859,02,14), Acronym = "OR", Name = "Oregon" },
            new() { StateId = 34, Date = new DateOnly(1861,01,29), Acronym = "KS", Name = "Kansas" },
            new() { StateId = 35, Date = new DateOnly(1863,06,20), Acronym = "WV", Name = "West Virginia" },
            new() { StateId = 36, Date = new DateOnly(1864,10,31), Acronym = "NV", Name = "Nevada" },
            new() { StateId = 37, Date = new DateOnly(1867,03,01), Acronym = "NE", Name = "Nebraska" },
            new() { StateId = 38, Date = new DateOnly(1876,08,01), Acronym = "CO", Name = "Colorado" },
            new() { StateId = 39, Date = new DateOnly(1889,11,02), Acronym = "ND", Name = "North Dakota" },
            new() { StateId = 40, Date = new DateOnly(1889,11,02), Acronym = "SD", Name = "South Dakota" },
            new() { StateId = 41, Date = new DateOnly(1889,11,08), Acronym = "MT", Name = "Montana" },
            new() { StateId = 42, Date = new DateOnly(1889,11,11), Acronym = "WA", Name = "Washington" },
            new() { StateId = 43, Date = new DateOnly(1890,07,03), Acronym = "ID", Name = "Idaho" },
            new() { StateId = 44, Date = new DateOnly(1890,07,10), Acronym = "WY", Name = "Wyoming" },
            new() { StateId = 45, Date = new DateOnly(1896,01,04), Acronym = "UT", Name = "Utah" },
            new() { StateId = 46, Date = new DateOnly(1907,11,16), Acronym = "OK", Name = "Oklahoma" },
            new() { StateId = 47, Date = new DateOnly(1912,01,06), Acronym = "NM", Name = "New Mexico" },
            new() { StateId = 48, Date = new DateOnly(1912,02,14), Acronym = "AZ", Name = "Arizona" },
            new() { StateId = 49, Date = new DateOnly(1959,01,03), Acronym = "AK", Name = "Alaska" },
            new() { StateId = 50, Date = new DateOnly(1959,08,21), Acronym = "HI", Name = "Hawaii" }
        };
    }
}