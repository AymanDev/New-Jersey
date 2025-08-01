using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Gui.Components;

public class Button : IDraw, IUpdate
{
    private string _text;
    private int _textWidth;

    private int _x;
    private int _y;
    private int _padding;
    private int _fontSize;

    private int _width;
    private int _height;

    public delegate void OnClickHandler();

    public event OnClickHandler OnClick;

    public bool IsPressed { get; private set; }

    public Button(string text, int x, int y, int fontSize, int padding = 10)
    {
        _text = text;
        _x = x;
        _y = y;
        _padding = padding;
        _fontSize = fontSize;

        Recalculate();
    }

    public void ChangeText(string text)
    {
        _text = text;
        Recalculate();
    }


    public bool IsMouseOver()
    {
        var mouse = Raylib.GetMousePosition();

        var isInsideHorizontal = mouse.X >= _x && mouse.X <= _x + _width;
        var isInsideVertically = mouse.Y >= _y && mouse.Y <= _y + _height;

        return isInsideHorizontal && isInsideVertically;
    }

    public void Update()
    {
        if (!IsMouseOver())
        {
            IsPressed = false;
            return;
        }

        IsPressed = Raylib.IsMouseButtonDown(MouseButton.Left);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            OnClick.Invoke();
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangle(_x, _y, _width, _height, IsPressed ? Color.DarkGray : Color.Gray);
        Raylib.DrawRectangleLines(_x, _y, _width, _height, Color.RayWhite);

        Raylib.DrawText(_text, _x + _padding, _y + _padding, _fontSize, Color.RayWhite);
    }

    private void Recalculate()
    {
        _textWidth = Raylib.MeasureText(_text, _fontSize);
        _width = _textWidth + _padding * 2;
        _height = _fontSize + _padding * 2;
    }
}