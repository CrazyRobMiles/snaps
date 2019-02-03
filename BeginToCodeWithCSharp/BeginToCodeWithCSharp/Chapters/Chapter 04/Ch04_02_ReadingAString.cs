using SnapsLibrary;

class Ch04_02_ReadingAString
{
    public void StartProgram()
    {
        string guestName;
        guestName = SnapsEngine.ReadString("What is your name");
        SnapsEngine.DisplayString(guestName);
    }
}
