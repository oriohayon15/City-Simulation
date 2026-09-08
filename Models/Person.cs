namespace CitySimulation.Models;

public class Person
{
    public int Age { get; set; }
    public Tile HomeBlock { get; set; }
    public Tile? WorkBlock { get; set; }
    public Tile? SchoolBlock { get; set; }
    public bool IsEmployed { get; set; }

    public Person(int age, Tile homeBlock)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(age);

        Age = age;
        HomeBlock = homeBlock ?? throw new ArgumentNullException(nameof(homeBlock));
    }
}
