Run the city simulation with `dotnet run --project CitySimulation.csproj`.
Choose any tile by entering its row and column, counting from the upper-left
(for example, `7 4`), to see its current capacity, occupancy, and utility
details. The selection is cleared when the simulation advances. Press Enter to
advance one year, or type `q` to quit. Each year ages residents,
applies deaths and births, updates school enrollment and teacher jobs, and
reassigns available work. The console reports the new population, births, and
deaths before showing the updated grid.
