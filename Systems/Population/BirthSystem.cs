using CitySimulation.Models;

public class BirthSystem
{
    public static int BirthFormula(City city, Random? random = null)
    {
        ArgumentNullException.ThrowIfNull(city);
        random ??= Random.Shared;

        var newBorns = new List<Person>();
        foreach (var person in city.Population)
        {
            if (person.Age >= 22 && person.Age <= 40)
            {
                if (random.NextDouble() < 0.06)
                {
                    newBorns.Add(new Person(0, person.HomeBlock));
                }
            }
        }

        HousingSystem.HousingFormula(city, newBorns);
        return newBorns.Count;
    }
}