using NewJersey.Engine.Gui.Components;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Gui;

public class DebugGui : IUpdate, IDraw
{
    private readonly Button _editorBtn;

    public DebugGui()
    {
        _editorBtn = new Button("Activate editor", Raylib.GetScreenWidth() - 200, 12, 20);
        _editorBtn.OnClick += HandleEditorButton;
    }

    private void HandleEditorButton()
    {
        Engine.Instance.IsInEditorMode = !Engine.Instance.IsInEditorMode;

        if (Engine.Instance.IsInEditorMode)
        {
            _editorBtn.ChangeText("Deactivate editor");
        }
        else
        {
            _editorBtn.ChangeText("Activate editor");
        }
    }


    public void Update()
    {
        _editorBtn.Update();
    }


    public void Draw()
    {
        _editorBtn.Draw();

        if (!Engine.Instance.IsInEditorMode)
        {
            return;
        }

        var offset = 12;
        Raylib.DrawText($"New Jersey", 12, offset, 20, Color.White);
        Raylib.DrawText("Performance ", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"FPS: {Raylib.GetFPS()}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Frame Time: {Raylib.GetFrameTime()}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Map", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Width: {Engine.Instance.Grid.Width}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Height: {Engine.Instance.Grid.Height}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Size: {Engine.Instance.Grid.Tiles.Length}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Commands", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Logic: {Engine.Instance.CommandsManager.LogicCommandsLength}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Graphics: {Engine.Instance.CommandsManager.GraphicsCommandsLength}", 12, offset += 20, 20, Color.White);


        var mousePosInWorld = Engine.Instance.Camera.FromCameraToWorld(Raylib.GetMousePosition());

        Raylib.DrawText("Mouse", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Position: {Raylib.GetMousePosition()}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"World Position: {mousePosInWorld}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Grid Position: <{Engine.Instance.GridX}, {Engine.Instance.GridY}>", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Inside map: {Engine.Instance.IsMouseInsideMap()}", 12, offset += 20, 20, Color.White);

        Raylib.DrawText("Camera", 12, offset += 40, 20, Color.White);
        Raylib.DrawText($"Target: {Engine.Instance.Camera.Target}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Offset: {Engine.Instance.Camera.Offset}", 12, offset += 20, 20, Color.White);
        Raylib.DrawText($"Zoom: {Engine.Instance.Camera.Zoom}", 12, offset += 20, 20, Color.White);
    }
}