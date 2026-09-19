using CitySimulation.Configuration;
using CitySimulation.Enums;
using CitySimulation.Models;


public static class EmploymentSystem
{
    public static void EmploymentFormula(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        var workplaceTiles = city.Grid.Cast<Tile>()
            .Where(tile => tile.Type == TileType.Workplace)
            .ToArray();

        //create a list of the all unemployed people in the order of oldest first so the oldest get first priority for a job
        var unemployedPeople = new List<Person>(
            city.Population.Where(person => 
            person.WorkBlock is null && person.Category == AgeCategory.Adult)
            .OrderByDescending(person => person.Age)
        );

        var occupancy = workplaceTiles.ToDictionary(tile => tile, _ => 0);
        

        foreach (var person in city.Population)
        {
            if (person.WorkBlock is not null && occupancy.ContainsKey(person.WorkBlock))
        {
            occupancy[person.WorkBlock]++;
        }
        }

        //iterate through the list and workplace tiles and add people to the workplace if there is space
        foreach (var person in unemployedPeople)
        {
            foreach (var workplace in workplaceTiles)
            {
                if (occupancy[workplace] < CitySettings.WorkplaceCapacity)
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
