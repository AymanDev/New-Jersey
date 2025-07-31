using ImGuiNET;
using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Gui;

public class DebugGui : IUpdate, IDraw
{
    public void Update()
    {
    }

    public void Begin()
    {
    }

    public void Draw()
    {
        // rlImGui.Begin();
        ImGui.ShowDemoWindow();
        Raylib.Button
        // rlImGui.End();
    }

    public void End()
    {
    }
}