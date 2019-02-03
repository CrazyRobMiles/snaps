using SnapsLibrary;

class Ch04_07_CentigradeAndFahrenheit
{
    public void StartProgram()
    {
        int tempInFahrenheit = 54;
        double tempInCentigrade = (tempInFahrenheit - 32) / 1.8;
        SnapsEngine.DisplayString("Fahreneheit: " + tempInFahrenheit);
        SnapsEngine.Delay(2);
        SnapsEngine.DisplayString("Centigrade: " + tempInCentigrade);
    }
}