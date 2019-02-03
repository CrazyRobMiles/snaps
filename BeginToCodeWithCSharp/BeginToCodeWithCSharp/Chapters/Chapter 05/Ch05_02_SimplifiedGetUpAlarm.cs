using SnapsLibrary;

class Ch05_02_SimplifiedGetUpAlarm
{
    public void StartProgram()
    {
        if (SnapsEngine.GetHourValue() > 6)
            SnapsEngine.DisplayString("It is time to get up");
    }
}
