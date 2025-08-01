using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Gui;

public class LoadingGui : IDraw
{
    private const string Title = "New Jersey";
    private string _status = "Initializing";


    public void Draw()
    {
        var widthCenter = Raylib.GetScreenWidth() / 2;
        var heightCenter = Raylib.GetScreenHeight() / 2;

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        Raylib.DrawText(Title, widthCenter - (Raylib.MeasureText(Title, 40) / 2), heightCenter, 40, Color.RayWhite);
        Raylib.DrawText(_status, widthCenter - (Raylib.MeasureText(_status, 20) / 2), heightCenter + 40, 20, Color.RayWhite);

        Raylib.EndDrawing();
    }

    public void ProgressNext(string status = "Initializing")
    {
        _status = status;

        Draw();
    }
}