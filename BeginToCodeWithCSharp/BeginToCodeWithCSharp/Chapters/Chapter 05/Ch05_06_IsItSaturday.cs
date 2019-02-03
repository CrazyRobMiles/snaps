using SnapsLibrary;

class Ch05_06_IsItSaturday
{
    public void StartProgram()
    {
        if (SnapsEngine.GetDayOfWeekName() == "Saturday")
            SnapsEngine.DisplayString("Yay! It's Saturday");
    }
}
