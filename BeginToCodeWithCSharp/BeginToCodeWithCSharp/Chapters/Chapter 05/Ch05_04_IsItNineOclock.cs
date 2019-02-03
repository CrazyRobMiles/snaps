using SnapsLibrary;

class Ch05_04_IsItNineOclock
{
    public void StartProgram()
    {
        if (SnapsEngine.GetHourValue() == 9)
            SnapsEngine.DisplayString("Nine hours and all well");
    }
}
