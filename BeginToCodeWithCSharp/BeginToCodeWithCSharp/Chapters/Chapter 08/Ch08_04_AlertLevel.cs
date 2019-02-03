using SnapsLibrary;

class Ch08_04_AlertLevel
{
    void Alert(string message, bool urgent)
    {
        if (urgent)
            SnapsEngine.PlaySoundEffect("ding");
        SnapsEngine.DisplayString(message);
        SnapsEngine.SpeakString(message);
    }

    public void StartProgram()
    {
        Alert("Time for a coffee break.", false);
        Alert("Reactor going critical.", true);

        Alert(urgent: false, message: "Donuts have arrived");
    }
}
