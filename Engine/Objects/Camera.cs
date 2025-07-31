using System.Numerics;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Objects;

public class Camera : IUpdate
{
    private Camera2D _camera;

    public Camera()
    {
        _camera = new Camera2D
        {
            Zoom = 1f
        };
    }


    public void Update()
    {
        var wheel = Raylib.GetMouseWheelMoveV().Y;
        _camera.Zoom += wheel * 20 * Raylib.GetFrameTime();

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


        _camera.Target += dir;
    }

    public void Begin()
    {
        Raylib.BeginMode2D(_camera);
    }

    public void End()
    {
        Raylib.EndMode2D();
    }
}