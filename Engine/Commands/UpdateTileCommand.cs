namespace NewJersey.Engine.Commands;

public class UpdateTileCommand(ref Tile tile) : BaseCommand(CommandType.Graphics)
{
    private readonly Tile _tile = tile;

    public override void Execute(Engine engine)
    {
        engine.WorldRenderer.UpdateTile(_tile);
    }
}