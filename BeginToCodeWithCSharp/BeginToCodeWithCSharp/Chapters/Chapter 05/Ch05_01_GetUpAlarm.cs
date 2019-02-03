using SnapsLibrary;

class Ch05_01_GetUpAlarm
{
    public void StartProgram()
    {
        int hourValue = SnapsEngine.GetHourValue();
        bool ItIsTimeToGetUp = hourValue > 6;
        if (ItIsTimeToGetUp)
            SnapsEngine.DisplayString("Time to get up");
    }
}
