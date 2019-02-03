using SnapsLibrary;

class Ch05_11_WhenIsThisMessagePrinted
{
    public void StartProgram()
    {
        if (SnapsEngine.GetDayOfWeekName() == "Saturday" & SnapsEngine.GetHourValue() > 8)
            SnapsEngine.DisplayString("It is time to get up");
        else
            SnapsEngine.DisplayString("When is this message printed?");
    }
}
