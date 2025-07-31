using Raylib_cs.BleedingEdge;

namespace NewJersey.Engine;

public  class CountriesManager
{
    public  Dictionary<uint, Country> Countries { get; } = new();

    private  uint _lastCountryId = 1;

    public  Country CreateCountry(Color color)
    {
        if (Countries.ContainsKey(_lastCountryId))
        {
            _lastCountryId++;
        }

        var country = new Country
        {
            Id = _lastCountryId,
            Color = color,
        };

        Countries.Add(country.Id, country);

        return country;
    }
}