using CitySimulation.Configuration;

namespace CitySimulation.Models;

public class City
{
    // Access tiles as Grid[x, y], with (0, 0) at the top-left corner.
    public Tile[,] Grid { get; } = new Tile[CitySettings.GridWidth, CitySettings.GridHeight];
    public List<Person> Population { get; } = [];

    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);

    public City()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Grid[x, y] = new Tile(x, y);
            }
        }
    }
}
