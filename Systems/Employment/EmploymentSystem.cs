using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;


public static class EmploymentSystem
{
    public static void EmploymentFormula(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        foreach (var person in city.Population)
        {
            if (person.Category != AgeCategory.Adult)
            {
                person.WorkBlock = null;
                person.IsEmployed = false;
            }
        }

        var workplaceTiles = city.Grid.Cast<Tile>()
            .Where(tile => tile.Type is TileType.Workplace or TileType.School)
            // Fill teaching vacancies before assigning other jobs.
            .OrderBy(tile => tile.Type == TileType.School ? 0 : 1)
            .ToArray();

        var occupancy = workplaceTiles.ToDictionary(tile => tile, _ => 0);
        

        foreach (var person in city.Population)
        {
            if (person.WorkBlock is not null
                && occupancy.TryGetValue(person.WorkBlock, out var count)
                && count < (person.WorkBlock.Type == TileType.School
                    ? CitySettings.TeachersPerSchool
                    : CitySettings.WorkplaceCapacity))
            {
                occupancy[person.WorkBlock]++;
                person.IsEmployed = true;
            }
            else
            {
                person.WorkBlock = null;
                person.IsEmployed = false;
            }
        }

        // Older unemployed adults get first priority for either kind of job.
        var unemployedPeople = city.Population
            .Where(person => person.WorkBlock is null && person.Category == AgeCategory.Adult)
            .OrderByDescending(person => person.Age);

        //iterate through the list and workplace tiles and add people to the workplace if there is space
        foreach (var person in unemployedPeople)
        {
            foreach (var workplace in workplaceTiles)
            {
                var capacity = workplace.Type == TileType.School
                    ? CitySettings.TeachersPerSchool
                    : CitySettings.WorkplaceCapacity;

                if (occupancy[workplace] < capacity)
                {
                    person.WorkBlock = workplace;
                    person.IsEmployed = true;
                    occupancy[workplace]++;
                    break;
                }
            }
        }
    }
}
