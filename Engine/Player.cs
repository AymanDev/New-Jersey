namespace NewJersey.Engine;

public class Player
{
    public uint CountryId { get; private set; }

    public Player(uint countryId)
    {
        CountryId = countryId;
    }
}