using ZLinq;

namespace NewJersey.Engine
{
    public enum TileType : uint
    {
        Void = 0,
        Water = 1,
        Sand = 2,
        Grass = 3,
        Forest = 4,
        Hill = 5,
        Mountain = 6,
        HighMountain = 7,
    }

    public struct TileTypeRange(int start, int end, TileType type)
    {
        public readonly int Start = start;
        public readonly int End = end;
        public readonly TileType Type = type;
    }


    public class TileTypeHelper
    {
        private readonly TileTypeRange[] _ranges =
        [
            new(start: 0, end: 106, type: TileType.Water),
            new(start: 107, end: 120, type: TileType.Sand),
            new(start: 121, end: 150, type: TileType.Grass),
            new(start: 151, end: 170, type: TileType.Forest),
            new(start: 171, end: 190, type: TileType.Hill),
            new(start: 191, end: 210, type: TileType.Mountain),
            new(start: 211, end: 255, type: TileType.HighMountain)
        ];

        public TileType GetTypeFromHeight(int height)
        {
            return (from range in _ranges where height >= range.Start && height <= range.End select range.Type).AsValueEnumerable().FirstOrDefault();
        }
    }

    public struct Tile
    {
        public int X;
        public int Y;
        public TileType Type;
        public uint CountryId;
    }
}