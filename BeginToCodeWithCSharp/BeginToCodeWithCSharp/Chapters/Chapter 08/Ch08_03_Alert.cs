using SnapsLibrary;

class Ch08_03_Alert
{
    void Alert(string message)
    {
        SnapsEngine.DisplayString(message);
        SnapsEngine.SpeakString(message);
    }

    public void StartProgram()
    {
        Alert("Reactor going critical.");
    }
}
