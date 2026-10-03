using CitySimulation.ConsoleUI;
using CitySimulation.Initialization;
using CitySimulation.Models;
using CitySimulation.Simulation;

namespace CitySimulation;

internal static class Program
{
    private static void Main()
    {
        var city = CityInitializer.CreateStartingCity();
        Console.WriteLine($"Starting population: {city.Population.Count:N0}");
        Console.WriteLine();
        CityGridRenderer.Render(city);

        var simulation = new YearlySimulation();

        while (true)
        {
            Console.WriteLine();
            Console.Write("Select a tile (row column), press Enter for the next year, or type q to quit: ");
            var command = Console.ReadLine();
            if (command is null || command.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (!string.IsNullOrWhiteSpace(command))
            {
                if (TryGetTile(city, command) is { } tile)
                {
                    CityGridRenderer.RenderTileStats(city, tile);
                }
                else
                {
                    Console.WriteLine(
                        $"Enter a row from 1 to {city.Height} and a column from 1 to {city.Width}, such as 7 4.");
                }

                continue;
            }

            try
            {
                var result = simulation.AdvanceOneYear(city);
                Console.WriteLine($"{result.Year}: {result.Population:N0} residents, "
                    + $"{result.Births:N0} births, {result.Deaths:N0} deaths");
                CityGridRenderer.Render(city);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"The city cannot advance: {exception.Message}");
                break;
            }
        }
    }

    private static Tile? TryGetTile(City city, string command)
    {
        var coordinates = command
            .Split([' ', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (coordinates.Length == 2
            && int.TryParse(coordinates[0], out var row)
            && int.TryParse(coordinates[1], out var column)
            && row >= 1 && row <= city.Height
            && column >= 1 && column <= city.Width)
        {
            return city.Grid[column - 1, row - 1];
        }

        return null;
    }
}
