// using System.Numerics;
using Raylib_cs.BleedingEdge;
// using ZLinq;

namespace NewJersey.Engine.Shaders;

public class CountryShader : IShader, IDestroy
{
    // public bool IsValid { get; private set; }

    private readonly Shader _shader;

    public CountryShader()
    {
        _shader = Raylib.LoadShader(null!, "Game/Assets/Shaders/country.fs");
    }

    // private readonly uint _bufferObj;
    // private readonly uint _bufferSize;

    // public unsafe CountryShader(Grid grid, CountriesManager countriesManager)
    // {
    //     _shader = Raylib.LoadShader(null!, "Game/Assets/Shaders/country.fs");
    //
    //     var widthLoc = Raylib.GetShaderLocation(_shader, "width");
    //     Raylib.SetShaderValue(_shader, widthLoc, grid.Width, ShaderUniformDataType.Int);
    //
    //     var heightLoc = Raylib.GetShaderLocation(_shader, "height");
    //     Raylib.SetShaderValue(_shader, heightLoc, grid.Height, ShaderUniformDataType.Int);
    //
    //
    //     for (uint id = 0; id < 128; id++)
    //     {
    //         var loc = Raylib.GetShaderLocation(_shader, $"countries[{id}]");
    //
    //         var color = new Vector3(1f, 0, 0);
    //
    //         if (countriesManager.Countries.TryGetValue(id, out var value))
    //         {
    //             var country = value.Color;
    //             color = new Vector3(country.R / (float) byte.MaxValue, country.G / (float) byte.MaxValue, country.B / (float) byte.MaxValue);
    //         }
    //
    //         Raylib.SetShaderValue(_shader, loc, color, ShaderUniformDataType.Vec3);
    //     }
    //
    //
    //     _bufferSize = (uint) grid.Tiles.Length * sizeof(uint);
    //     _bufferObj = Rlgl.LoadShaderBuffer(_bufferSize, null, RlglEnum.DynamicRead);
    //
    //     UpdateBuffer(grid);
    //
    //
    //     IsValid = Raylib.IsShaderValid(_shader);
    //
    //     Raylib.TraceLog(TraceLogLevel.Info, $"[Country Shader] Is Valid: {IsValid}");
    // }

    // private unsafe void UpdateBuffer(Grid grid)
    // {
    //     var tiles = grid.Tiles.AsValueEnumerable().Select(t => t.CountryId).ToArray();
    //
    //     fixed (uint* countryIds = tiles)
    //     {
    //         Rlgl.UpdateShaderBuffer(_bufferObj, countryIds, _bufferSize, 0);
    //     }
    // }
    //
    // public unsafe void PartialUpdate(ref Tile tile)
    // {
    //     var idx = Engine.Instance.Grid.GetIndexFromXAndY(tile.X, tile.Y);
    //
    //
    //     fixed (uint* id = &tile.CountryId)
    //     {
    //         Rlgl.UpdateShaderBuffer(_bufferObj, id, sizeof(uint), (uint) idx * sizeof(uint));
    //     }
    // }
    //

    public void Bind()
    {
        Raylib.BeginShaderMode(_shader);
        // Rlgl.BindShaderBuffer(_bufferObj, 5);
    }

    public void Destroy()
    {
        // Rlgl.UnloadShaderBuffer(_bufferObj);
        Raylib.UnloadShader(_shader);
    }
}