using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;

public static class SchoolSystem
{
    public static void SchoolFormula(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        var schools = city.Grid.Cast<Tile>()
            .Where(tile => tile.Type == TileType.School)
            .ToArray();
        var emptyTiles = new Queue<Tile>(city.Grid.Cast<Tile>()
            .Where(tile => tile.Type == TileType.Empty));
        var studentCount = city.Population.Count(person => person.Category == AgeCategory.Student);
        var maximumCapacity = (long)(schools.Length + emptyTiles.Count) * CitySettings.SchoolCapacity;

        // Check before changing enrollment or land, just as the housing system does.
        if (studentCount > maximumCapacity)
        {
            throw new InvalidOperationException(
                "There are not enough schools and empty land for the city's students.");
        }

        var occupancy = schools.ToDictionary(tile => tile, _ => 0);
        var studentsToEnroll = new List<Person>();

        foreach (var person in city.Population)
        {
            if (person.Category != AgeCategory.Student)
            {
                person.SchoolBlock = null;
            }
            else if (person.SchoolBlock is not null
                && occupancy.TryGetValue(person.SchoolBlock, out var count)
                && count < CitySettings.SchoolCapacity)
            {
                occupancy[person.SchoolBlock]++;
            }
            else
            {
                studentsToEnroll.Add(person);
            }
        }

        var availableSchools = new Queue<Tile>(schools
            .Where(tile => occupancy[tile] < CitySettings.SchoolCapacity));

        foreach (var student in studentsToEnroll)
        {
            if (availableSchools.Count == 0)
            {
                var newSchool = emptyTiles.Dequeue();
                newSchool.Type = TileType.School;
                occupancy[newSchool] = 0;
                availableSchools.Enqueue(newSchool);
            }

            var school = availableSchools.Peek();
            student.SchoolBlock = school;
            occupancy[school]++;

            if (occupancy[school] == CitySettings.SchoolCapacity)
            {
                availableSchools.Dequeue();
            }
        }

        // Open the next school as soon as all existing schools reach capacity.
        if (occupancy.Count > 0 && availableSchools.Count == 0 && emptyTiles.Count > 0)
        {
            emptyTiles.Dequeue().Type = TileType.School;
        }
    }
}
