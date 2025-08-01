using NewJersey.Engine.Commands;
using NewJersey.Engine.Gui;
using NewJersey.Engine.Objects;
using NewJersey.Engine.Renderer;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine;

public unsafe class Engine : IDestroy, IUpdate
{
    private readonly Camera _camera;
    public static Engine Instance { get; private set; }

    public readonly WorldRenderer WorldRenderer;

    public bool IsInEditorMode = false;


    public readonly TileTypeHelper TileTypeHelper = new();
    public readonly CountriesManager CountriesManager = new();
    public readonly CommandsManager CommandsManager = new();

    public Grid Grid { get; private set; }

    private static Texture2D _mapTexture;

    public Engine()
    {
        Instance = this;
        Raylib.SetConfigFlags(ConfigFlags.WindowResizable);
        Raylib.SetTraceLogLevel(TraceLogLevel.All);
        Raylib.InitWindow(1280, 720, "New Jersey");
        Raylib.SetTargetFPS(3000);

        var loadingGui = new LoadingGui();
        loadingGui.Draw();

        _camera = new Camera();

        loadingGui.ProgressNext("Loading map image...");
        var image = Raylib.LoadImage("Game/Assets/earth.png");
        _mapTexture = Raylib.LoadTextureFromImage(image);

        loadingGui.ProgressNext("Generating grid...");

        var width = image.Width;
        var height = image.Height;

        Grid = new Grid(width, height);

        loadingGui.ProgressNext("Loading grid from the image...");

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


        // loadingGui.ProgressNext("Generating countries..");
        //
        // var myCountry = CountriesManager.CreateCountry(new Color(0, 255, 0));
        //
        // for (var x = 0; x < 100; x++)
        // {
        //     for (var y = 0; y < 100; y++)
        //     {
        //         ref var tile = ref Grid.GetTile(x, y);
        //         tile.CountryId = myCountry.Id;
        //     }
        // }
        //
        // CommandsManager.Enqueue(new AttackTileCommand(103, 103, myCountry.Id));

        loadingGui.ProgressNext("Preparing renderer...");

        WorldRenderer = new WorldRenderer();
    }


    public void Update()
    {
        CommandsManager.Update();
        _camera.Update();

        WorldRenderer.Update();
    }

    public void Draw()
    {
        var mousePosInWorld = _camera.FromCameraToWorld(Raylib.GetMousePosition());

        Raylib.BeginDrawing();

        _camera.Begin();

        Raylib.ClearBackground(Color.Black);

        WorldRenderer.Draw();


        Raylib.DrawCircleV(mousePosInWorld, 20, Color.Red);

        _camera.End();


        var offset = 12;
        Raylib.DrawText($"New Jersey", 12, offset, 20, Color.White);
        Raylib.DrawText("Performance ", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Frame Time: {Raylib.GetFrameTime()}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Map", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Width: {Grid.Width}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Height: {Grid.Height}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Size: {Grid.Tiles.Length}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Commands", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Logic: {CommandsManager.LogicCommandsLength}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Graphics: {CommandsManager.GraphicsCommandsLength}", 12, offset += 20, 20, Color.White);


        Raylib.DrawText("Mouse", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Position: {Raylib.GetMousePosition()}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"World Position: {mousePosInWorld}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Inside map: {IsMouseInsideMap()}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Camera", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Target: {_camera.Target}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Offset: {_camera.Offset}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Zoom: {_camera.Zoom}", 12, offset += 20, 20, Color.White);


        Raylib.EndDrawing();
    }

    public void Destroy()
    {
        WorldRenderer.Draw();
        // rlImGui.Shutdown();
        Raylib.UnloadTexture(_mapTexture);

        Raylib.CloseWindow();
    }


    public bool IsMouseInsideMap()
    {
        var mousePosInWorld = _camera.FromCameraToWorld(Raylib.GetMousePosition());

        return mousePosInWorld.X > 0 && mousePosInWorld.X <= Grid.Width && mousePosInWorld.Y > 0 && mousePosInWorld.Y <= Grid.Height;
    }
}