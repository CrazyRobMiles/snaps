using SnapsLibrary;

class Ch04_09_TemperatureDisplay
{
    public void StartProgram()
    {
int temperature =
    SnapsEngine.GetTodayTemperatureInFahrenheit(latitude: 47.61, longitude: -122.33);
        string fullMessage;
        fullMessage = "The current temperature is " + temperature + " degrees.";
        SnapsEngine.DisplayString(fullMessage);
    }
}
