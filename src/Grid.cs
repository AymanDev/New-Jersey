namespace New_Jersey.src
{
    class Grid(int width, int height)
    {
        public Tile[] tiles = new Tile[width * height];

        private readonly int width = width;
        private readonly int height = height;

        private int GetIndexFromXZ(int x, int y)
        {
            return y * width + x;
        }

        public void SetTile(Tile tile)
        {
            var idx = GetIndexFromXZ(tile.x, tile.y);

            tiles[idx] = tile;
        }

        public TileType[] GetTileTypes()
        {
            return [.. this.tiles.Select(t => t.type)];
        }
    }
}