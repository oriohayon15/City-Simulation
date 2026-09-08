using CitySimulation.Enums;

namespace CitySimulation.Models;

public class Tile
{
    public int X { get; }
    public int Y { get; }
    public TileType Type { get; set; }

    public Tile(int x, int y, TileType type = TileType.Empty)
    {
        X = x;
        Y = y;
        Type = type;
    }
}
