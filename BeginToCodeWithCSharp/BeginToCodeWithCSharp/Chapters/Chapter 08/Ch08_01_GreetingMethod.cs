using SnapsLibrary;

class Ch08_01_GreetingMethod
{
    void Greeting()
    {
        SnapsEngine.SpeakString("Hello");
    }

    public void StartProgram()
    {
        Greeting();
    }
}
