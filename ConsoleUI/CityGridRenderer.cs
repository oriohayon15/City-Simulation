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
}
