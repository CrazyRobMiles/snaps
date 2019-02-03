using SnapsLibrary;

class Ch04_10_WeatherConditionsDisplay
{
    public void StartProgram()
    {
        string conditions = SnapsEngine.GetWeatherConditionsDescription(latitude: 47.61, longitude: -122.33);
        SnapsEngine.DisplayString(conditions);
    }
}