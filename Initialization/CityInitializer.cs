using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;

namespace CitySimulation.Initialization;

public static class CityInitializer
{
    public static City CreateStartingCity()
    {
        var city = new City();

        (TileType Type, int Count)[] landDistribution =
        [
            (TileType.Residential, CitySettings.InitialResidentialBlocks),
            (TileType.Workplace, CitySettings.InitialWorkplaceBlocks),
            (TileType.School, CitySettings.InitialSchoolBlocks),
            (TileType.Empty, CitySettings.InitialEmptyBlocks)
        ];

        if (landDistribution.Any(group => group.Count < 0)
            || CitySettings.InitialWaterBlocks < 0
            || landDistribution.Sum(group => group.Count) + CitySettings.InitialWaterBlocks
                != city.Width * city.Height)
        {
            throw new InvalidOperationException("Starting block counts must fill the city grid exactly.");
        }

        var landTypes = landDistribution
            .SelectMany(group => Enumerable.Repeat(group.Type, group.Count))
            .ToArray();

        // Shuffle only the land tiles. A fixed seed makes the layout random-looking
        // but reproducible, which is useful when testing the simulation.
        new Random(CitySettings.InitialCityLayoutSeed).Shuffle(landTypes);

        for (var tileIndex = 0; tileIndex < city.Width * city.Height; tileIndex++)
        {
            var x = tileIndex % city.Width;
            var y = tileIndex / city.Width;

            city.Grid[x, y].Type = tileIndex < landTypes.Length
                ? landTypes[tileIndex]
                : TileType.Water;
        }

        PopulationInitializer.Initialize(city);
        EmploymentSystem.EmploymentFormula(city);

        return city;
    }
}
