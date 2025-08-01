using NewJersey.Engine.Commands;
using NewJersey.Engine.Gui;
using NewJersey.Engine.Objects;
using NewJersey.Engine.Renderer;
using Raylib_cs.BleedingEdge;
using ZLinq;

namespace NewJersey.Engine;

public unsafe class Engine : IDestroy, IUpdate
{
    public Camera Camera { get; }
    public static Engine Instance { get; private set; }

    public readonly WorldRenderer WorldRenderer;

    private DebugGui _debugGui;
    private InGameGui _inGameGui;

    public bool IsInEditorMode = false;

    public Player? Player { get; private set; }


    public readonly TileTypeHelper TileTypeHelper = new();
    public readonly CountriesManager CountriesManager = new();
    public readonly CommandsManager CommandsManager = new();

    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public Grid Grid { get; private set; }

    private static Texture2D _mapTexture;

    public Engine()
    {
        Instance = this;
        Raylib.SetConfigFlags(ConfigFlags.WindowResizable);
        Raylib.SetTraceLogLevel(TraceLogLevel.All);
        Raylib.InitWindow(1280, 720, "New Jersey");
        Raylib.SetTargetFPS(3000);

        _debugGui = new DebugGui();
        _inGameGui = new InGameGui();

        var loadingGui = new LoadingGui();
        loadingGui.Draw();

        Camera = new Camera();

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
        Camera.Update();
        WorldRenderer.Update();
        _debugGui.Update();

        var mouseWorldPos = Camera.FromCameraToWorld(Raylib.GetMousePosition());

        GridX = (int) Math.Ceiling(mouseWorldPos.X);
        GridY = (int) Math.Ceiling(mouseWorldPos.Y);

        GridX = Math.Max(Math.Min(GridX, Grid.Width), 0);
        GridY = Math.Max(Math.Min(GridY, Grid.Height), 0);


        if (IsInEditorMode)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                var country = CountriesManager.CreateCountry(new Color(255, 0, 0));

                ref var tile = ref Grid.GetTile(GridX, GridY);
                tile.CountryId = country.Id;

                Player = new Player(country.Id);
            }
        }

        if (!IsInEditorMode)
        {
            _inGameGui.Update();

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (Player is null)
                {
                    return;
                }
                var country = CountriesManager.Countries.AsValueEnumerable().First().Key;
                CommandsManager.Enqueue(new AttackTileCommand(GridX, GridY, country));
            }
        }
    }

    public void Draw()
    {
        Raylib.BeginDrawing();

        Camera.Begin();

        Raylib.ClearBackground(Color.Black);

        WorldRenderer.Draw();


        // Raylib.DrawCircleV(mousePosInWorld, 20, Color.Red);

        Camera.End();

        _debugGui.Draw();

        if (!IsInEditorMode)
        {
            _inGameGui.Draw();
        }

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
        var mousePosInWorld = Camera.FromCameraToWorld(Raylib.GetMousePosition());

        return mousePosInWorld.X > 0 && mousePosInWorld.X <= Grid.Width && mousePosInWorld.Y > 0 && mousePosInWorld.Y <= Grid.Height;
    }
}