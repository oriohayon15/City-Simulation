using CitySimulation.ConsoleUI;
using CitySimulation.Initialization;
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
            Console.Write("Press Enter for the next year, or type q to quit: ");
            var command = Console.ReadLine();
            if (command is null || command.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (!string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine("Enter a blank line to advance, or q to quit.");
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
}
