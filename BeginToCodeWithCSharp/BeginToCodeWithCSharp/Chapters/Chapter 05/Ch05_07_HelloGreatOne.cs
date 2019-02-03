using SnapsLibrary;

class Ch05_07_HelloGreatOne
{
    public void StartProgram()
    {
        string name;
        name = SnapsEngine.ReadString("What is your name");
        if (name == "Rob")
            SnapsEngine.DisplayString("Hello, Oh great one");
    }
}