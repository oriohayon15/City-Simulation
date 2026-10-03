using CitySimulation.Enums;
using CitySimulation.Models;

namespace CitySimulation.ConsoleUI;

public static class CityGridRenderer
{
    public static void Render(City city)
    {
        Console.WriteLine($"Year: {city.CurrentYear}");
        Console.WriteLine($"City grid ({city.Width} x {city.Height})");
        Console.WriteLine();

        var useColor = !Console.IsOutputRedirected;
        var originalColor = useColor ? Console.ForegroundColor : default;

        try
        {
            for (var y = 0; y < city.Height; y++)
            {
                for (var x = 0; x < city.Width; x++)
                {
                    var (symbol, color) = city.Grid[x, y].Type switch
                    {
                        TileType.Residential => ('\u2588', ConsoleColor.Green),
                        TileType.Workplace => ('\u2588', ConsoleColor.Yellow),
                        TileType.School => ('\u2588', ConsoleColor.Magenta),
                        TileType.Water => ('\u2588', ConsoleColor.Blue),
                        _ => ('\u2588', ConsoleColor.DarkGray)
                    };

                    if (useColor)
                    {
                        Console.ForegroundColor = color;
                    }

                    Console.Write($"{symbol} ");
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
        finally
        {
            if (useColor)
            {
                Console.ForegroundColor = originalColor;
            }
        }

        Console.WriteLine();
        Console.WriteLine("\u001b[32m\u2588\u001b[0m = Residential, \u001b[33m\u2588\u001b[0m = Workplace, \u001b[35m\u2588\u001b[0m = School, \u001b[34m\u2588\u001b[0m = Water, \u001b[90m\u2588\u001b[0m = Empty");
    }

    public static void RenderTileStats(City city, Tile tile)
    {
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(tile);

        Console.WriteLine();
        Console.WriteLine($"Selected tile — Row: {tile.Y + 1}, Column: {tile.X + 1}");
        Console.WriteLine($"Type: {tile.Type}");

        switch (tile.Type)
        {
            case TileType.Residential:
                Console.WriteLine($"Maximum capacity: {Configuration.CitySettings.ResidentialCapacity:N0}");
                Console.WriteLine($"Current residents: {city.Population.Count(person => ReferenceEquals(person.HomeBlock, tile)):N0}");
                Console.WriteLine($"Water attached: {FormatBoolean(tile.HasWater)}");
                break;

            case TileType.Workplace:
                Console.WriteLine($"Maximum capacity: {Configuration.CitySettings.WorkplaceCapacity:N0}");
                Console.WriteLine($"Current workers: {city.Population.Count(person => person.IsEmployed && ReferenceEquals(person.WorkBlock, tile)):N0}");
                Console.WriteLine($"Water attached: {FormatBoolean(tile.HasWater)}");
                break;

            case TileType.School:
                Console.WriteLine($"Maximum student capacity: {Configuration.CitySettings.SchoolCapacity:N0}");
                Console.WriteLine($"Active students: {city.Population.Count(person => ReferenceEquals(person.SchoolBlock, tile)):N0}");
                Console.WriteLine($"Active teachers: {city.Population.Count(person => person.IsTeacher && ReferenceEquals(person.WorkBlock, tile)):N0}");
                Console.WriteLine($"Maximum teachers: {Configuration.CitySettings.TeachersPerSchool:N0}");
                Console.WriteLine($"Water attached: {FormatBoolean(tile.HasWater)}");
                break;

            case TileType.Water:
                Console.WriteLine("Water source: Yes");
                break;

            case TileType.Empty:
                Console.WriteLine("This tile is currently undeveloped.");
                Console.WriteLine("Water attached: No");
                break;
        }
    }

    private static string FormatBoolean(bool value) => value ? "Yes" : "No";
}
