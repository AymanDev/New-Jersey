using NewJersey.Engine.Commands;
using NewJersey.Engine.Gui;
using NewJersey.Engine.Objects;
using NewJersey.Engine.Shaders;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine;

public unsafe class Engine : IDestroy, IUpdate
{
    private readonly Camera _camera;
    public static Engine Instance { get; private set; }

    // private readonly DebugGui _debugGui = new();


    public readonly TileTypeHelper TileTypeHelper = new();
    public readonly CountriesManager CountriesManager = new();
    public readonly CommandsManager CommandsManager = new();

    private readonly ElevationShader _elevationShader;
    public CountryShader CountryShader { get; private set; }

    public Grid Grid { get; private set; }

    private static Texture2D _mapTexture;

    public Engine()
    {

        Instance = this;
        Raylib.SetTraceLogLevel(TraceLogLevel.All);
        Raylib.InitWindow(1280, 720, "New Jersey");
        Raylib.SetTargetFPS(3000);
        // rlImGui.Setup(true);


        _camera = new Camera();



        var image = Raylib.LoadImage("Game/Assets/earth.png");
        _mapTexture = Raylib.LoadTextureFromImage(image);

        var width = image.Width;
        var height = image.Height;

        Grid = new Grid(width, height);

        var colors = Raylib.LoadImageColors(image);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var idx = (y * width) + x;
                var value = colors[idx].R;

                var type = TileTypeHelper.GetTypeFromHeight(value);

                Grid.SetTile(new Tile()
                {
                    Type = type,
                    X = x,
                    Y = y,
                    CountryId = 0
                });
            }
        }

        Raylib.UnloadImageColors(colors);
        Raylib.UnloadImage(image);


        var myCountry = CountriesManager.CreateCountry(new Color(0, 255, 0));

        for (var x = 0; x < 100; x++)
        {
            for (var y = 0; y < 100; y++)
            {
                ref var tile = ref Grid.GetTile(x, y);
                tile.CountryId = myCountry.Id;
            }
        }

        CommandsManager.Enqueue(new AttackBaseCommand(103, 103, myCountry.Id));


        _elevationShader = new ElevationShader(Grid);
        CountryShader = new CountryShader(Grid, CountriesManager);
        
    }


    public void Update()
    {
        // _debugGui.Update();
        CommandsManager.Update();

        _camera.Update();
    }

    public void Draw()
    {
        
        Raylib.BeginDrawing();

        _camera.Begin();

        Raylib.ClearBackground(Color.Black);


        _elevationShader.Bind();
        Raylib.DrawTexture(_mapTexture, 0, 0, Color.White);

        CountryShader.Bind();
        Raylib.DrawTexture(_mapTexture, 0, 0, Color.White);
        Raylib.EndShaderMode();


        _camera.End();


        var offset = 12;
        Raylib.DrawText($"New Jersey", 12, offset, 20, Color.White);
        offset += 20;
        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 12, offset, 20, Color.White);
        offset += 20;
        Raylib.DrawText($"Is Shader Valid: {_elevationShader.IsValid}", 12, offset, 20, Color.White);
        offset += 20;

        Raylib.DrawText($"Map Width: {Grid.Width}", 12, offset, 20, Color.White);
        offset += 20;
        Raylib.DrawText($"Map Height: {Grid.Height}", 12, offset, 20, Color.White);
        offset += 20;
        Raylib.DrawText($"Map Size: {Grid.Tiles.Length}", 12, offset, 20, Color.White);

        offset += 20;
        Raylib.DrawText($"Logic Commands: {CommandsManager.LogicCommandsLength}", 12, offset, 20, Color.White);
        offset += 20;
        Raylib.DrawText($"Graphics Commands: {CommandsManager.GraphicsCommandsLength}", 12, offset, 20, Color.White);


        // _debugGui.Draw();
        Raylib.EndDrawing();
    }

    public void Destroy()
    {
        // rlImGui.Shutdown();
        Raylib.UnloadTexture(_mapTexture);

        _elevationShader.Destroy();

        Raylib.CloseWindow();
    }
}