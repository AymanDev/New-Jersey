using ZLinq;

namespace NewJersey.Engine
{
    public class Grid(int width, int height)
    {
        public Tile[] Tiles { get; } = new Tile[width * height];

        public int Width => width;
        public int Height => height;

        public int GetIndexFromXAndY(int x, int y)
        {
            return y * width + x;
        }

        public void SetTile(Tile tile)
        {
            var idx = GetIndexFromXAndY(tile.X, tile.Y);

            Tiles[idx] = tile;
        }

        public ref Tile GetTile(int x, int y)
        {
            return ref Tiles[GetIndexFromXAndY(x, y)];
        }
        

        public TileType[] GetTileTypes()
        {
            return this.Tiles.AsValueEnumerable().Select(t => t.Type).ToArray();
        }
    }
}