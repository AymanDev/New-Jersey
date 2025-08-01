using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine.Gui;

public class InGameGui : IDraw, IUpdate
{
    public void Update()
    {
    }

    public void Draw()
    {
        var offset = 12;

        Raylib.DrawText("Countries", 12, offset, 20, Color.White);

        if (Engine.Instance.Player is not null)
        {
            Raylib.DrawText($"My Country: {Engine.Instance.Player.CountryId}", 12, offset += 20, 20, Color.White);
        }

        foreach (var country in Engine.Instance.CountriesManager.Countries.Values)
        {
            Raylib.DrawText($"Country {country.Id}", 12, offset += 20, 20, Color.White);
        }
    }
}