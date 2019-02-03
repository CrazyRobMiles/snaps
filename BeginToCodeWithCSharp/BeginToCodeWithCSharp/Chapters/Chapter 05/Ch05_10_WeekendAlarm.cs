using SnapsLibrary;

class Ch05_10_WeekendAlarm
{
    public void StartProgram()
    {
        if (SnapsEngine.GetDayOfWeekName() == "Saturday" & SnapsEngine.GetHourValue() > 8)
            SnapsEngine.DisplayString("It is time to get up");
    }
}
