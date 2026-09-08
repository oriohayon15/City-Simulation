using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;

namespace CitySimulation.Initialization;

public static class CityInitializer
{
    public static City CreateStartingCity()
    {
        var city = new City();

        (TileType Type, int Count)[] distribution =
        [
            (TileType.Residential, CitySettings.InitialResidentialBlocks),
            (TileType.Workplace, CitySettings.InitialWorkplaceBlocks),
            (TileType.Water, CitySettings.InitialWaterBlocks),
            (TileType.School, CitySettings.InitialSchoolBlocks),
            (TileType.Empty, CitySettings.InitialEmptyBlocks)
        ];

        if (distribution.Any(group => group.Count < 0)
            || distribution.Sum(group => group.Count) != city.Width * city.Height)
        {
            throw new InvalidOperationException("Starting block counts must fill the city grid exactly.");
        }

        // Fill left to right, then top to bottom for a predictable initial layout.
        var tileIndex = 0;

        foreach (var (type, count) in distribution)
        {
            for (var i = 0; i < count; i++)
            {
                var x = tileIndex % city.Width;
                var y = tileIndex / city.Width;
                city.Grid[x, y].Type = type;
                tileIndex++;
            }
        }

        PopulationInitializer.Initialize(city);

        return city;
    }
}
