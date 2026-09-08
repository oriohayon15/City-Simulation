# City Simulation

A single C# console project based on [City Project Plan.pdf](City%20Project%20Plan.pdf).
Uses .NET 10 with no additional packages or UI frameworks.

The initial city grid and 10,000-person population are implemented, with a colored
console display. Tile capacities, simulation rules, keyboard controls, and a test
suite are still to be added.

## Run

Install a .NET 10.0.3xx SDK, then run these commands from this directory:

```sh
dotnet build
dotnet run
```

The program prints a 12x12 grid and a legend, then exits. Colors are enabled in an
interactive terminal; symbols also make the grid readable in redirected output.

## City grid

- `TileType` defines Residential, Workplace, School, Water, and Empty.
- `Tile` stores its fixed zero-based `X` and `Y` coordinates and changeable type.
- `City` creates 144 distinct empty tiles, accessed as `city.Grid[x, y]`.
  `(0, 0)` is the top-left corner; X increases to the right and Y increases downward.
- `CityInitializer.CreateStartingCity()` assigns the plan's starting distribution:
  25 residential, 20 workplace, 25 water, 5 school, and 69 empty blocks.
- `CitySettings` holds the dimensions and starting block counts. Initialization
  checks that the counts are non-negative and fill the grid exactly.

## Starting population

- `Person` stores age, home, optional workplace and school assignments, and
  employment status.
- `City.Population` contains all citizens.
- `PopulationInitializer` creates exactly 10,000 citizens using the plan's age
  distribution: 20% ages 0-17, 35% ages 18-40, 30% ages 41-65, and 15% ages
  66-100. The upper bound of 100 is an explicit assumption because the plan only
  specifies 66+.
- Citizens are assigned evenly across the 25 residential blocks (400 per block).
- A fixed default random seed makes initial ages reproducible.

The starting layout randomly mixes residential, workplace, school, and empty
tiles, while reserving the final 25 grid positions for water. A fixed layout seed
keeps the result reproducible; changing `InitialCityLayoutSeed` creates a different
layout. `ConsoleUI/CityGridRenderer` displays the grid without changing the city
model.

## Folders

- `Models/`: Tile, City, and Person.
- `Enums/`: Tile types and age categories.
- `Configuration/`: Grid size, initial population, capacities, and rates.
- `Initialization/`: Starting city and population setup.
- `Simulation/`: Yearly simulation flow and Mayor coordination.
- `Systems/Population/`: Aging, births, and deaths.
- `Systems/Housing/`: Housing capacity and assignments.
- `Systems/Employment/`: Jobs and employment assignments.
- `Systems/Education/`: School capacity and student assignments.
- `Systems/Water/`: Water availability.
- `ConsoleUI/`: Colored text grid; statistics and keyboard controls will be added later.

Empty folders contain `.gitkeep` files so version control can preserve them.
The console approach uses keyboard input; the plan's clickable block details
would require a graphical interface later.
