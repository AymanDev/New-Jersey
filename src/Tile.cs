
using System.Linq;

namespace New_Jersey
{
    public enum TileType
    {
        Void = 0,
        Water = 1,
        Sand = 2,
        Grass = 3,
        Forest = 4,
        Hill = 5,
        Mountain = 6,
        HightMountain = 7,

    }

    public struct TileTypeRange
    {
        public int start;
        public int end;
        public TileType type;
    }


    class TileTypeHelper
    {

        TileTypeRange[] ranges = [];

        public TileTypeHelper()
        {
            ranges = [
                new TileTypeRange{
                    start=0,
                    end=106,
                    type = TileType.Water,
                },
                new TileTypeRange{
                    start=107,
                    end=120,
                    type = TileType.Sand,
                },
                new TileTypeRange{
                    start=121,
                    end=150,
                    type = TileType.Grass,
                },
                new TileTypeRange{
                    start=151,
                    end=170,
                    type = TileType.Forest,
                },
                new TileTypeRange{
                    start=171,
                    end=190,
                    type = TileType.Hill,
                },
                new TileTypeRange{
                    start=191,
                    end=210,
                    type = TileType.Mountain,
                },
                new TileTypeRange{
                    start=211,
                    end=255,
                    type = TileType.HightMountain,
                },
            ];
        }
        public TileType GetTypeFromHeight(int height)
        {
            foreach (var range in ranges)
            {
                if (height >= range.start && height <= range.end)
                {
                    return range.type;
                }
            }

            return TileType.Void;
        }
    }

    struct Tile
    {
        public int x;
        public int y;
        public TileType type;

    }
}