namespace NewJersey.Engine.Renderer;

public class WorldRenderer : IDraw, IDestroy,IUpdate
{
    private readonly ElevationMapRenderer _elevationMap = new(Engine.Instance.Grid);
    private readonly CountryMapRenderer _countriesMap = new(Engine.Instance.Grid, Engine.Instance.CountriesManager);


    public void UpdateTile(Tile tile)
    {
        _countriesMap.UpdateTile(tile);
    }
    
    public void Update()
    {
        _countriesMap.Update();
    }
    
    public void Draw()
    {
        _elevationMap.Draw();
        _countriesMap.Draw();
    }

    public void Destroy()
    {
        _elevationMap.Destroy();
        _countriesMap.Destroy();
    }

}