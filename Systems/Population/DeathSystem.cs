using CitySimulation.Models;

public class DeathSystem
{
    public static int DeathFormula(City city, Random? random = null)
    {
        ArgumentNullException.ThrowIfNull(city);
        random ??= Random.Shared;

        return city.Population.RemoveAll(person =>
        (person.Age < 50 && random.NextDouble() < 0.001) ||
        (person.Age >=  50 && person.Age < 66 && random.NextDouble() < 0.0025) ||
        (person.Age >= 66 && person.Age < 80 && random.NextDouble() < 0.005) ||
        (person.Age >= 80 && person.Age < 90 && random.NextDouble() < 0.04) ||
        (person.Age > 89 && random.NextDouble() < 0.08));
    }
}
