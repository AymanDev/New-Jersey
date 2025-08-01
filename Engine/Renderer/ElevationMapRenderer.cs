using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Renderer;

public class ElevationMapRenderer : IDraw, IDestroy
{
    private readonly Texture2D _elevationTexture;

    public ElevationMapRenderer(Grid grid)
    {
        var elevationImage = Raylib.GenImageColor(grid.Width, grid.Height, Color.Red);

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var tile = grid.GetTile(x, y);
                var newColor = GetColorForTileType(tile.Type);

                Raylib.ImageDrawPixel(ref elevationImage, x, y, newColor);
            }
        }

        _elevationTexture = Raylib.LoadTextureFromImage(elevationImage);

        Raylib.UnloadImage(elevationImage);
    }

    private Color GetColorForTileType(TileType type)
    {
        return type switch
        {
            TileType.Water => Color.Blue,
            TileType.Sand => Color.Yellow,
            TileType.Grass => Color.Green,
            TileType.Forest => new Color(0, 153, 0),
            TileType.Hill => new Color(179, 153, 0),
            TileType.Mountain => new Color(102, 102, 102),
            TileType.HighMountain => Color.White,
            _ => Color.Red
        };
    }


    public void Draw()
    {
        Raylib.DrawTexture(_elevationTexture, 0, 0, Color.White);
    }


    public void Destroy()
    {
        Raylib.UnloadTexture(_elevationTexture);
    }
}