using SnapsLibrary;

class Ch05_08_GreatOneUpperCase
{
    public void StartProgram()
    {
        string name;
        name = SnapsEngine.ReadString("What is your name");
        string upperCaseName = name.ToUpper();
        if (upperCaseName == "ROB")
            SnapsEngine.DisplayString("Hello, Oh great one");
    }
}