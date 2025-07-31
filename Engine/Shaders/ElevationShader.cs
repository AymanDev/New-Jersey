using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Shaders;

public class ElevationShader : IShader, IDestroy
{
    public bool IsValid { get; private set; }

    private readonly Shader _shader;
    private readonly uint _bufferObj;


    public unsafe ElevationShader(Grid grid)
    {
        _shader = Raylib.LoadShader(null!, "Game/Assets/Shaders/elevation.fs");

        var widthLoc = Raylib.GetShaderLocation(_shader, "width");
        Raylib.SetShaderValue(_shader, widthLoc, grid.Width, ShaderUniformDataType.Int);

        var heightLoc = Raylib.GetShaderLocation(_shader, "height");
        Raylib.SetShaderValue(_shader, heightLoc, grid.Height, ShaderUniformDataType.Int);

        var bufferSize = (uint) grid.Tiles.Length * sizeof(uint);
        _bufferObj = Rlgl.LoadShaderBuffer(bufferSize, null, RlglEnum.DynamicRead);

        fixed (TileType* p = grid.GetTileTypes())
        {
            Rlgl.UpdateShaderBuffer(_bufferObj, p, bufferSize, 0);
        }

        IsValid = Raylib.IsShaderValid(_shader);
    }

    public void Bind()
    {
        Raylib.BeginShaderMode(_shader);
        Rlgl.BindShaderBuffer(_bufferObj, 5);
    }

    public void Destroy()
    {
        Rlgl.UnloadShaderBuffer(_bufferObj);
        Raylib.UnloadShader(_shader);
    }
}