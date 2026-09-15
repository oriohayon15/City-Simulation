using CitySimulation.Models;

public class BirthSystem
{
    public static void BirthFormula(City city)
    {
        List<Person> newBorns = new List<Person>();
        Random random = new Random();
        foreach (Person person in city.Population)
        {
            if (person.Age >= 22 && person.Age <= 40)
                {
                    double randomBirth = random.NextDouble();
                    if (randomBirth < 0.02)
                    {
                        newBorns.Add(new Person(0, person.HomeBlock));
                    }
                }
        }

        HousingSystem.HousingFormula(city, newBorns);
    }
}