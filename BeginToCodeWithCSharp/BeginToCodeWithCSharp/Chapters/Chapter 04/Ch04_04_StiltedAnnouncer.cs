using SnapsLibrary;

class Ch04_04_StiltedAnnouncer
{
    public void StartProgram()
    {
        string guestName;
        guestName = SnapsEngine.ReadString("What is your name");
        SnapsEngine.SpeakString("The honorable Mister");
        SnapsEngine.SpeakString(guestName);
    }
}
