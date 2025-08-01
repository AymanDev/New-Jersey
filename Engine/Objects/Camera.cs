using System.Numerics;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Objects;

public class Camera : IUpdate
{
    private Camera2D _camera = new()
    {
        Zoom = 1f,
    };

    public float Zoom => _camera.Zoom;
    public Vector2 Target => _camera.Target;
    public Vector2 Offset => _camera.Offset;

    private float _targetZoom;

    public void Update()
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            MoveWithMouse();
        }
        else
        {
            MoveWithKeyboard();
        }

        PerformZoom();
    }

    private void MoveWithMouse()
    {
        var delta = Raylib.GetMouseDelta();
        delta *= -1.0f / _camera.Zoom;
        _camera.Target += delta;
    }

    private void MoveWithKeyboard()
    {
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


        dir *= -1.0f / _camera.Zoom;
        _camera.Target -= dir;
    }


    private void PerformZoom()
    {
        var wheel = Raylib.GetMouseWheelMoveV().Y;

        if (Math.Abs(wheel) == 0)
        {
            return;
        }

        _targetZoom += wheel * 0.05f;
        _targetZoom = Math.Min(Math.Max(_targetZoom, 0.5f), 5f);
        _camera.Target = FromCameraToWorld(Raylib.GetMousePosition());
        _camera.Offset = Raylib.GetMousePosition();
        _camera.Zoom = _targetZoom;
    }

    public Vector2 FromCameraToWorld(Vector2 position)
    {
        return Raylib.GetScreenToWorld2D(position, _camera);
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