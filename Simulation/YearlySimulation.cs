using CitySimulation.Models;

namespace CitySimulation.Simulation;

public sealed class YearlySimulation
{
    private readonly Random _random;

    public YearlySimulation(Random? random = null)
    {
        _random = random ?? Random.Shared;
    }

    public YearResult AdvanceOneYear(City city)
    {
        ArgumentNullException.ThrowIfNull(city);

        var people = city.Population.ToArray();
        var personStates = people.Select(person => new PersonState(
            person.Age, person.HomeBlock, person.WorkBlock, person.SchoolBlock, person.IsEmployed)).ToArray();
        var tileStates = city.Grid.Cast<Tile>().Select(tile => (Tile: tile, Type: tile.Type)).ToArray();

        try
        {
            foreach (var person in people)
            {
                person.AgeOneYear();
            }

            var deaths = DeathSystem.DeathFormula(city, _random);
            // Aging and deaths change enrollment before newborns need housing.
            SchoolSystem.SchoolFormula(city);
            var births = BirthSystem.BirthFormula(city, _random);
            EmploymentSystem.EmploymentFormula(city);

            city.CurrentYear++;
            return new YearResult(city.CurrentYear, births, deaths, city.Population.Count);
        }
        catch
        {
            foreach (var (tile, type) in tileStates)
            {
                tile.Type = type;
            }

            city.Population.Clear();
            city.Population.AddRange(people);
            for (var i = 0; i < people.Length; i++)
            {
                var person = people[i];
                var state = personStates[i];
                person.RestoreAge(state.Age);
                person.HomeBlock = state.HomeBlock;
                person.WorkBlock = state.WorkBlock;
                person.SchoolBlock = state.SchoolBlock;
                person.IsEmployed = state.IsEmployed;
            }

            throw;
        }
    }

    private readonly record struct PersonState(
        int Age, Tile HomeBlock, Tile? WorkBlock, Tile? SchoolBlock, bool IsEmployed);
}

public readonly record struct YearResult(int Year, int Births, int Deaths, int Population);
