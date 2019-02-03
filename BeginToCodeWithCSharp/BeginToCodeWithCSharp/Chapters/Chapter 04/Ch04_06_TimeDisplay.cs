using SnapsLibrary;

class Ch04_06_TimeDisplay
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Current Time");
        int hourValue = SnapsEngine.GetHourValue();
        int minuteValue = SnapsEngine.GetMinuteValue();
        SnapsEngine.DisplayString(hourValue + ":" + minuteValue);
    }
}