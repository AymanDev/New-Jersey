using NewJersey.Engine.Shaders;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Renderer;

public class CountryMapRenderer : IDraw, IDestroy, IUpdate
{
    private readonly Texture2D _texture;
    private readonly unsafe Color* _pixels;
    private readonly CountryShader _shader;
    private readonly Color _defaultColor = new(0, 0, 0, 0);
    private float _lastTimeUpdated;

    private const float TextureUpdateEvery = 0.05f;

    public unsafe CountryMapRenderer(Grid grid, CountriesManager countries)
    {
        var image = Raylib.GenImageColor(grid.Width, grid.Height, Color.Red);

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var tile = grid.GetTile(x, y);
                var owner = tile.CountryId;

                var color = GetColorForCountryId(owner);
                Raylib.ImageDrawPixel(ref image, x, y, color);
            }
        }

        _pixels = Raylib.LoadImageColors(image);
        _texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);

        _shader = new CountryShader();
    }


    public unsafe void Update()
    {
        _lastTimeUpdated += Raylib.GetFrameTime();

        if (_lastTimeUpdated < TextureUpdateEvery)
        {
            return;
        }

        _lastTimeUpdated = 0;
        Raylib.UpdateTexture(_texture, _pixels);
    }


    public unsafe void UpdateTile(Tile tile)
    {
        var color = GetColorForCountryId(tile.CountryId);

        _pixels[Engine.Instance.Grid.GetIndexFromXAndY(tile.X, tile.Y)] = color;

        // This introduces huge performance costs as updating texture
        // also known as sending texture to gpu in vram is costly procedure
        // especially doing so every frame
        // this will reduce fps from ~2700-3000
        // to ~90-140
        // Raylib.UpdateTexture(_texture, _pixels);
    }

    public void Draw()
    {
        _shader.Bind();
        Raylib.DrawTexture(_texture, 0, 0, Color.White);
        Raylib.EndShaderMode();
    }

    public unsafe void Destroy()
    {
        _shader.Destroy();
        Raylib.UnloadImageColors(_pixels);
        Raylib.UnloadTexture(_texture);
    }

    private Color GetColorForCountryId(uint countryId)
    {
        if (countryId <= 0)
        {
            return _defaultColor;
        }

        var country = Engine.Instance.CountriesManager.Countries[countryId];
        return country.Color;
    }
}