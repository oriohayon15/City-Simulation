using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;

public static class HousingSystem
{
    // New residents are added only after enough housing has been found for everyone.
    public static void HousingFormula(City city, IReadOnlyCollection<Person>? newResidents = null)
    {
        ArgumentNullException.ThrowIfNull(city);

        var residentialTiles = city.Grid.Cast<Tile>()
            .Where(tile => tile.Type == TileType.Residential)
            .ToArray();
        var emptyTiles = new Queue<Tile>(city.Grid.Cast<Tile>()
            .Where(tile => tile.Type == TileType.Empty));

        var populationSize = (long)city.Population.Count + (newResidents?.Count ?? 0);
        var maximumCapacity = (long)(residentialTiles.Length + emptyTiles.Count) * CitySettings.ResidentialCapacity;

        if (populationSize > maximumCapacity)
        {
            throw new InvalidOperationException(
                "There is not enough housing and empty land for the city's population.");
        }

        var occupancy = residentialTiles.ToDictionary(tile => tile, _ => 0);
        var peopleToMove = new List<Person>();
        IEnumerable<Person> residents;
        if (newResidents is null)
        {
            residents = city.Population.AsEnumerable();
        }
        else
        {
            residents = city.Population.Concat(newResidents);
        }

        // Existing residents are counted first, so newcomers move when a tile is full.
        foreach (var person in residents)
        {
            if (occupancy.TryGetValue(person.HomeBlock, out var count) && count < CitySettings.ResidentialCapacity)
            {
                occupancy[person.HomeBlock]++;
            }
            else
            {
                peopleToMove.Add(person);
            }
        }

        var availableHomes = new Queue<Tile>(residentialTiles.Where(tile => occupancy[tile] < CitySettings.ResidentialCapacity));

        foreach (var person in peopleToMove)
        {
            if (availableHomes.Count == 0)
            {
                var newHome = emptyTiles.Dequeue();
                newHome.Type = TileType.Residential;
                occupancy[newHome] = 0;
                availableHomes.Enqueue(newHome);
            }

            var home = availableHomes.Peek();
            person.HomeBlock = home;
            occupancy[home]++;

            if (occupancy[home] == CitySettings.ResidentialCapacity)
            {
                availableHomes.Dequeue();
            }
        }

        if (newResidents is not null)
        {
            city.Population.AddRange(newResidents);
        }
    }
}
