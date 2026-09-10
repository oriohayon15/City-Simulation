using System.Runtime.InteropServices.Marshalling;
using CitySimulation.Models;

public class DeathSystem
{
    public static void DeathFormula(City city)
    {
        Random random = new Random();

        city.Population.RemoveAll(person => 
        (person.Age < 50 && random.NextDouble() < 0.001) ||
        (person.Age >=  50 && person.Age < 66 && random.NextDouble() < 0.005) ||
        (person.Age >= 66 && person.Age < 80 && random.NextDouble() < 0.02) ||
        (person.Age >= 80 && person.Age < 90 && random.NextDouble() < 0.08) ||
        (person.Age > 89 && random.NextDouble() < 0.20));
    }
}
