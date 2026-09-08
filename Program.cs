using CitySimulation.ConsoleUI;
using CitySimulation.Initialization;

namespace CitySimulation;

internal static class Program
{
    private static void Main()
    {
        var city = CityInitializer.CreateStartingCity();
        Console.WriteLine($"Starting population: {city.Population.Count:N0}");
        Console.WriteLine();
        CityGridRenderer.Render(city);
    }
}
