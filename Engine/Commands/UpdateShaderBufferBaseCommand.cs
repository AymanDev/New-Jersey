namespace NewJersey.Engine.Commands;

public class UpdateShaderBufferBaseCommand(ref Tile tile) : BaseCommand(CommandType.Graphics)
{
    private Tile _tile = tile;

    public override void Execute(Engine engine)
    {
        engine.CountryShader.PartialUpdate(ref _tile);
    }
}