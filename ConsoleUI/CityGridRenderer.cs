using CitySimulation.Enums;
using CitySimulation.Models;

namespace CitySimulation.ConsoleUI;

public static class CityGridRenderer
{
    public static void Render(City city)
    {
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
                        TileType.Residential => ('R', ConsoleColor.Green),
                        TileType.Workplace => ('W', ConsoleColor.Yellow),
                        TileType.School => ('S', ConsoleColor.Magenta),
                        TileType.Water => ('~', ConsoleColor.Blue),
                        _ => ('.', ConsoleColor.DarkGray)
                    };

                    if (useColor)
                    {
                        Console.ForegroundColor = color;
                    }

                    Console.Write($"{symbol} ");
                }

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
        Console.WriteLine("R = Residential, W = Workplace, S = School, ~ = Water, . = Empty");
    }
}
