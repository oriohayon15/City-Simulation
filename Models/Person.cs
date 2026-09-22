using CitySimulation.Enums;

namespace CitySimulation.Models;

public class Person
{
    public int Age { get; private set; }
    public Tile HomeBlock { get; set; }
    public Tile? WorkBlock { get; set; }
    public Tile? SchoolBlock { get; set; }
    public bool IsEmployed { get; set; }
    public bool IsTeacher => IsEmployed && WorkBlock?.Type == TileType.School;
    public AgeCategory Category {get
        {
            if (Age <= 4) 
                return AgeCategory.Child;
            else if (Age <= 17) 
                return AgeCategory.Student;
            else if (Age <= 65) 
                return AgeCategory.Adult;
            else
                return AgeCategory.Retired;
        }}

    public Person(int age, Tile homeBlock)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(age);

        Age = age;
        HomeBlock = homeBlock ?? throw new ArgumentNullException(nameof(homeBlock));
    }

    public void AgeOneYear() => Age++;

    internal void RestoreAge(int age) => Age = age;
}
