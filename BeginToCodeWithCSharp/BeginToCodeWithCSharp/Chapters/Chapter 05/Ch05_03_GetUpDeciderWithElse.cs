using SnapsLibrary;

class Ch05_03_GetUpDeciderWithElse
{
    public void StartProgram()
    {
        if (SnapsEngine.GetHourValue() < 9)
            SnapsEngine.DisplayString("Go back to sleep");
        else
            SnapsEngine.DisplayString("Time to get up");
    }

}
