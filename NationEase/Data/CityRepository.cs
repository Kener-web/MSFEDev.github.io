using NationEase.Models;

namespace NationEase.Data;

public static class CityRepository
{
    public static List<City> GetCities()
    {
    List<City> citiesList = new List<City>
    {
        new() { CityId = 1, StateId = 11, Name = "New York City", Population = 8804190m },
        new() { CityId = 2, StateId = 31, Name = "Los Angeles", Population = 3898747m },
        new() { CityId = 3, StateId = 21, Name = "Chicago", Population = 2746388m },
        new() { CityId = 4, StateId = 28, Name = "Houston", Population = 2304580m },
        new() { CityId = 5, StateId = 48, Name = "Phoenix", Population = 1608139m },
        new() { CityId = 6, StateId = 2, Name = "Philadelphia", Population = 1603797m },
        new() { CityId = 7, StateId = 27, Name = "Jacksonville", Population = 949611m },
        new() { CityId = 8, StateId = 17, Name = "Columbus", Population = 905748m },
        new() { CityId = 9, StateId = 19, Name = "Indianapolis", Population = 887642m },
        new() { CityId = 10, StateId = 12, Name = "Charlotte", Population = 874579m },
        new() { CityId = 11, StateId = 42, Name = "Seattle", Population = 737015m },
        new() { CityId = 12, StateId = 38, Name = "Denver", Population = 715522m },
        new() { CityId = 13, StateId = 16, Name = "Nashville", Population = 689447m },
        new() { CityId = 14, StateId = 46, Name = "Oklahoma City", Population = 681054m },
        new() { CityId = 15, StateId = 6, Name = "Boston", Population = 675647m },
        new() { CityId = 16, StateId = 33, Name = "Portland", Population = 652503m },
        new() { CityId = 17, StateId = 36, Name = "Las Vegas", Population = 641903m },
        new() { CityId = 18, StateId = 26, Name = "Detroit", Population = 639111m },
        new() { CityId = 19, StateId = 15, Name = "Louisville", Population = 633045m },
        new() { CityId = 20, StateId = 7, Name = "Baltimore", Population = 585708m },

        new() { CityId = 21, StateId = 30, Name = "Milwaukee", Population = 577222m },
        new() { CityId = 22, StateId = 47, Name = "Albuquerque", Population = 564559m },
        new() { CityId = 23, StateId = 24, Name = "Kansas City", Population = 508090m },
        new() { CityId = 24, StateId = 4, Name = "Atlanta", Population = 498715m },
        new() { CityId = 25, StateId = 37, Name = "Omaha", Population = 486051m },
        new() { CityId = 26, StateId = 10, Name = "Virginia Beach", Population = 459470m },
        new() { CityId = 27, StateId = 32, Name = "Minneapolis", Population = 429954m },
        new() { CityId = 28, StateId = 34, Name = "Wichita", Population = 397532m },
        new() { CityId = 29, StateId = 18, Name = "New Orleans", Population = 383997m },
        new() { CityId = 30, StateId = 50, Name = "Honolulu", Population = 350964m },
        new() { CityId = 31, StateId = 3, Name = "Newark", Population = 311549m },
        new() { CityId = 32, StateId = 49, Name = "Anchorage", Population = 291247m },
        new() { CityId = 33, StateId = 43, Name = "Boise", Population = 235684m },
        new() { CityId = 34, StateId = 22, Name = "Huntsville", Population = 215006m },
        new() { CityId = 35, StateId = 29, Name = "Des Moines", Population = 214133m },
        new() { CityId = 36, StateId = 25, Name = "Little Rock", Population = 202591m },
        new() { CityId = 37, StateId = 45, Name = "Salt Lake City", Population = 200133m },
        new() { CityId = 38, StateId = 40, Name = "Sioux Falls", Population = 192517m },
        new() { CityId = 39, StateId = 13, Name = "Providence", Population = 190934m },
        new() { CityId = 40, StateId = 20, Name = "Jackson", Population = 153701m },

        new() { CityId = 41, StateId = 8, Name = "Charleston", Population = 150227m },
        new() { CityId = 42, StateId = 5, Name = "Bridgeport", Population = 148654m },
        new() { CityId = 43, StateId = 39, Name = "Fargo", Population = 125990m },
        new() { CityId = 44, StateId = 41, Name = "Billings", Population = 117116m },
        new() { CityId = 45, StateId = 9, Name = "Manchester", Population = 115644m },
        new() { CityId = 46, StateId = 1, Name = "Wilmington", Population = 70898m },
        new() { CityId = 47, StateId = 23, Name = "Portland", Population = 68408m },
        new() { CityId = 48, StateId = 44, Name = "Cheyenne", Population = 65132m },
        new() { CityId = 49, StateId = 35, Name = "Charleston", Population = 48864m },
        new() { CityId = 50, StateId = 14, Name = "Burlington", Population = 44743m },

        // Small-population counterparts

        new() { CityId = 51, StateId = 32, Name = "Funkley", Population = 5m },
        new() { CityId = 52, StateId = 37, Name = "Monowi", Population = 1m },
        new() { CityId = 53, StateId = 44, Name = "Lost Springs", Population = 4m },
        new() { CityId = 54, StateId = 39, Name = "Ruso", Population = 4m },
        new() { CityId = 55, StateId = 40, Name = "Hillsview", Population = 2m },
        new() { CityId = 56, StateId = 41, Name = "Ismay", Population = 7m },
        new() { CityId = 57, StateId = 28, Name = "Guerra", Population = 6m },
        new() { CityId = 58, StateId = 34, Name = "Freeport", Population = 5m },
        new() { CityId = 59, StateId = 35, Name = "Thurmond", Population = 5m },
        new() { CityId = 60, StateId = 38, Name = "Carbonate", Population = 0m },

        new() { CityId = 61, StateId = 2, Name = "Centralia", Population = 5m },
        new() { CityId = 62, StateId = 11, Name = "Pine Valley", Population = 19m },
        new() { CityId = 63, StateId = 3, Name = "Teterboro", Population = 67m },
        new() { CityId = 64, StateId = 4, Name = "Edge Hill", Population = 24m },
        new() { CityId = 65, StateId = 5, Name = "Union", Population = 785m },
        new() { CityId = 66, StateId = 6, Name = "Gosnold", Population = 70m },
        new() { CityId = 67, StateId = 7, Name = "Brookview", Population = 60m },
        new() { CityId = 68, StateId = 8, Name = "Smyrna", Population = 45m },
        new() { CityId = 69, StateId = 9, Name = "Harts Location", Population = 68m },
        new() { CityId = 70, StateId = 10, Name = "Nassawadox", Population = 499m },

        new() { CityId = 71, StateId = 12, Name = "East Laurinburg", Population = 250m },
        new() { CityId = 72, StateId = 13, Name = "New Shoreham", Population = 1410m },
        new() { CityId = 73, StateId = 14, Name = "Victory", Population = 70m },
        new() { CityId = 74, StateId = 15, Name = "Bandana", Population = 35m },
        new() { CityId = 75, StateId = 16, Name = "Friendship", Population = 92m },
        new() { CityId = 76, StateId = 17, Name = "Rendville", Population = 36m },
        new() { CityId = 77, StateId = 18, Name = "Mound", Population = 12m },
        new() { CityId = 78, StateId = 19, Name = "Catesby", Population = 90m },
        new() { CityId = 79, StateId = 20, Name = "Satartia", Population = 41m },
        new() { CityId = 80, StateId = 21, Name = "Valley City", Population = 14m },

        new() { CityId = 81, StateId = 22, Name = "McMullen", Population = 10m },
        new() { CityId = 82, StateId = 23, Name = "Beddington", Population = 57m },
        new() { CityId = 83, StateId = 24, Name = "Goss", Population = 3m },
        new() { CityId = 84, StateId = 25, Name = "Victoria", Population = 20m },
        new() { CityId = 85, StateId = 26, Name = "Omer", Population = 274m },
        new() { CityId = 86, StateId = 27, Name = "Weeki Wachee", Population = 16m },
        new() { CityId = 87, StateId = 29, Name = "Beaconsfield", Population = 15m },
        new() { CityId = 88, StateId = 30, Name = "Big Falls", Population = 11m },
        new() { CityId = 89, StateId = 31, Name = "Vernon", Population = 222m },
        new() { CityId = 90, StateId = 33, Name = "Greenhorn", Population = 2m },

        new() { CityId = 91, StateId = 36, Name = "Cal-Nev-Ari", Population = 350m },
        new() { CityId = 92, StateId = 42, Name = "Krupp", Population = 49m },
        new() { CityId = 93, StateId = 43, Name = "Warm River", Population = 10m },
        new() { CityId = 94, StateId = 45, Name = "Scofield", Population = 23m },
        new() { CityId = 95, StateId = 46, Name = "Lotsee", Population = 2m },
        new() { CityId = 96, StateId = 47, Name = "Grenville", Population = 38m },
        new() { CityId = 97, StateId = 48, Name = "Winkelman", Population = 296m },
        new() { CityId = 98, StateId = 49, Name = "Bettles", Population = 50m },
        new() { CityId = 99, StateId = 50, Name = "Hana", Population = 757m },
        new() { CityId = 100, StateId = 1, Name = "Ardentown", Population = 264m }
       };

       return citiesList.
        OrderByDescending(c => c.Population)
        .ToList();
    }
}