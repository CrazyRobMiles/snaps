using SnapsLibrary;

class Ch04_05_SmoothAnnouncer
{
    public void StartProgram()
    {
        string guestName;
        guestName = SnapsEngine.ReadString("What is your name");
        string fullMessage = "The honorable Mister " + guestName;
        SnapsEngine.SpeakString(fullMessage);
    }
}
