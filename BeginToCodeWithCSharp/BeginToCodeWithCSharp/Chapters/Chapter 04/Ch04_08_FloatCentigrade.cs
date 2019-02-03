using SnapsLibrary;

class Ch04_08_FloatCentigrade
{
    public void StartProgram()
    {
        int tempInFahrenheit = 54;
        float tempInCentigrade = (tempInFahrenheit - 32) / 1.8f;
        SnapsEngine.DisplayString("Fahreneheit: " + tempInFahrenheit);
        SnapsEngine.Delay(2);
        SnapsEngine.DisplayString("Centigrade: " + tempInCentigrade);
    }
}