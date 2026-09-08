using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;

namespace CitySimulation.Initialization;

public static class PopulationInitializer
{
    public static void Initialize(City city, Random? random = null)
    {
        ArgumentNullException.ThrowIfNull(city);

        if (city.Population.Count > 0)
        {
            throw new InvalidOperationException("The city's population has already been initialized.");
        }

        var residentialBlocks = city.Grid
            .Cast<Tile>()
            .Where(tile => tile.Type == TileType.Residential)
            .ToArray();

        if (residentialBlocks.Length == 0)
        {
            throw new InvalidOperationException("Population initialization requires a residential block.");
        }

        var percentages = new[]
        {
            CitySettings.InitialAge0To17Percentage,
            CitySettings.InitialAge18To40Percentage,
            CitySettings.InitialAge41To65Percentage,
            CitySettings.InitialAge66PlusPercentage
        };

        if (percentages.Any(percentage => percentage < 0)
            || Math.Abs(percentages.Sum() - 1.0) > 0.000001)
        {
            throw new InvalidOperationException("Initial age percentages must be non-negative and total 100%.");
        }

        random ??= new Random(0);

        var groupCounts = percentages
            .Select(percentage => (int)(CitySettings.InitialPopulation * percentage))
            .ToArray();

        // Give any rounding remainder to the oldest group so the total is exact.
        groupCounts[^1] += CitySettings.InitialPopulation - groupCounts.Sum();

        AddAgeGroup(city, residentialBlocks, groupCounts[0], 0, 17, random);
        AddAgeGroup(city, residentialBlocks, groupCounts[1], 18, 40, random);
        AddAgeGroup(city, residentialBlocks, groupCounts[2], 41, 65, random);
        AddAgeGroup(city, residentialBlocks, groupCounts[3], 66, CitySettings.InitialMaximumAge, random);
    }

    private static void AddAgeGroup(
        City city,
        IReadOnlyList<Tile> residentialBlocks,
        int count,
        int minimumAge,
        int maximumAge,
        Random random)
    {
        for (var i = 0; i < count; i++)
        {
            var homeBlock = residentialBlocks[city.Population.Count % residentialBlocks.Count];
            var age = random.Next(minimumAge, maximumAge + 1);
            city.Population.Add(new Person(age, homeBlock));
        }
    }
}
