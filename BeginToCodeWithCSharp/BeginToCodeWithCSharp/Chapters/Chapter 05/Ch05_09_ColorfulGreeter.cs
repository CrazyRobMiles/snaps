using SnapsLibrary;

class Ch05_09_ColorfulGreeter
{
    public void StartProgram()
    {
        string name;
        name = SnapsEngine.ReadString("What is your name");
        string upperCaseName = name.ToUpper();
        if (upperCaseName == "ROB")
        {
            string dayOfWeek = SnapsEngine.GetDayOfWeekName();
            string fullMessage = "Hello Rob. Hope you are having a great " + dayOfWeek;
            SnapsEngine.SetTextColor(SnapsColor.Blue);
            SnapsEngine.DisplayString(fullMessage);
        }
    }
}