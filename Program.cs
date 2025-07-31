using System.Numerics;
using NewJersey.Engine;
using Raylib_cs.BleedingEdge;


// Honey
namespace NewJersey
{
    internal abstract class Program
    {
        public static void Main()
        {
            var engine = new Engine.Engine();

            while (!Raylib.WindowShouldClose())
            {
                engine.Update();

                engine.Draw();
            }

            engine.Destroy();
        }
    }
}