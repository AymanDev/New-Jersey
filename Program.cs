using System.Numerics;
using New_Jersey.src;
using Raylib_cs.BleedingEdge;

// Honey
namespace New_Jersey
{
    class Program
    {
        public unsafe static void Main()
        {
            Raylib.InitWindow(1280, 720, "New Jersey");

            var camera = new Camera2D
            {
                Zoom = 1.0f
            };

            Raylib.SetTargetFPS(3000);

            var image = Raylib.LoadImage("Game/Assets/earth.png");
            var texture = Raylib.LoadTextureFromImage(image);
            var width = image.Width;
            var height = image.Height;

            var time = 0f;

            var shader = Raylib.LoadShader(null, "Game/Assets/Shaders/elevation.fs");

            var timeLoc = Raylib.GetShaderLocation(shader, "time");
            Raylib.SetShaderValue(shader, timeLoc, time, ShaderUniformDataType.Float);

            var grid = new Grid(width, height);

            var colors = Raylib.LoadImageColors(image);

            var tileTypeHelper = new TileTypeHelper();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var idx = (y * width) + x;
                    var value = colors[idx].R;

                    var type = tileTypeHelper.GetTypeFromHeight(value);

                    grid.SetTile(new Tile()
                    {
                        type = type,
                        x = x,
                        y = y
                    });
                }
            }

            Raylib.UnloadImageColors(colors);
            Raylib.UnloadImage(image);

            var widthLoc = Raylib.GetShaderLocation(shader, "width");
            Raylib.SetShaderValue(shader, widthLoc, width, ShaderUniformDataType.Int);

            var heightLoc = Raylib.GetShaderLocation(shader, "height");
            Raylib.SetShaderValue(shader, heightLoc, height, ShaderUniformDataType.Int);

            var bufferSize = (uint)grid.tiles.Length * sizeof(uint);
            var ssbo = Rlgl.LoadShaderBuffer(bufferSize, null, RlglEnum.DynamicRead);

            fixed (TileType* p = grid.GetTileTypes())
            {
                Rlgl.UpdateShaderBuffer(ssbo, p, bufferSize, 0);
            }

            var isShaderValid = Raylib.IsShaderValid(shader);

            while (!Raylib.WindowShouldClose())
            {
                var wheel = Raylib.GetMouseWheelMoveV().Y;

                camera.Zoom += wheel * 20 * Raylib.GetFrameTime();

                var dir = new Vector2();

                if (Raylib.IsKeyDown(KeyboardKey.A))
                {
                    dir.X -= 1;
                }

                if (Raylib.IsKeyDown(KeyboardKey.D))
                {
                    dir.X += 1;
                }

                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    dir.Y -= 1;
                }

                if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    dir.Y += 1;
                }


                camera.Target += dir;


                Raylib.BeginDrawing();
                Raylib.BeginMode2D(camera);

                Raylib.ClearBackground(Color.Black);


                Raylib.BeginShaderMode(shader);
                Rlgl.BindShaderBuffer(ssbo, 5);

                Raylib.DrawTexture(texture, 0, 0, Color.White);

                Raylib.EndShaderMode();


                Raylib.EndMode2D();


                var offset = 12;
                Raylib.DrawText($"New Jersey", 12, offset, 20, Color.White);
                offset += 20;
                Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 12, offset, 20, Color.White);
                offset += 20;
                Raylib.DrawText($"Is Shader Valid: {isShaderValid}", 12, offset, 20, Color.White);
                offset += 20;

                Raylib.DrawText($"Map Width: {width}", 12, offset, 20, Color.White);
                offset += 20;
                Raylib.DrawText($"Map Height: {height}", 12, offset, 20, Color.White);
                offset += 20;
                Raylib.DrawText($"Map Size: {grid.tiles.Length}", 12, offset, 20, Color.White);


                Raylib.EndDrawing();
            }

            Raylib.UnloadTexture(texture);
            Raylib.UnloadShader(shader);

            Raylib.CloseWindow();
        }
    }
}