namespace NewJersey.Engine.Commands;

public class AttackTileCommand(int x, int y, uint attackerId) : BaseCommand(CommandType.Logic)
{
    private readonly int _x = x;
    private readonly int _y = y;
    private readonly uint _attackerId = attackerId;

    public override void Execute(Engine engine)
    {
        ref var tile = ref engine.Grid.GetTile(_x, _y);

        if (tile.CountryId == _attackerId)
        {
            return;
        }

        if (tile.Type == TileType.Water)
        {
            return;
        }

        tile.CountryId = _attackerId;

        engine.CommandsManager.Enqueue(new UpdateTileCommand(ref tile));

        const int range = 5;
        for (var xOffset = -range; xOffset < range; xOffset++)
        {
            for (var yOffset = -range; yOffset < range; yOffset++)
            {
                var neighborX = _x + xOffset;
                var neighborY = _y + yOffset;

                if (neighborX < 0 || neighborX >= engine.Grid.Width)
                {
                    continue;
                }

                if (neighborY < 0 || neighborY >= engine.Grid.Height)
                {
                    continue;
                }

                var neighbor = engine.Grid.GetTile(neighborX, neighborY);

                if (neighbor.CountryId == _attackerId)
                {
                    continue;
                }

                engine.CommandsManager.Enqueue(new AttackTileCommand(neighborX, neighborY, _attackerId));
            }
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AttackTileCommand command)
        {
            return false;
        }

        return Type == command.Type && _x == command._x && _y == command._y && _attackerId == command._attackerId;
    }

    public bool Equals(AttackTileCommand other)
    {
        return _x == other._x && _y == other._y && _attackerId == other._attackerId;
    }

    public override int GetHashCode()
    {
        return Engine.Instance.Grid.GetIndexFromXAndY(_x, _y);
    }
}