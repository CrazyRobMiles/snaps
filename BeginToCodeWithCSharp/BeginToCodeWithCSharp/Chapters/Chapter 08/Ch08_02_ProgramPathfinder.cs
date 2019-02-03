using SnapsLibrary;

class Ch08_02_ProgramPathfinder
{
    void M1()
    {
        M2();
        SnapsEngine.SpeakString("cat");
        M3();
        SnapsEngine.SpeakString("mat");
    }

    void M2()
    {
        SnapsEngine.SpeakString("the");
    }

    void M3()
    {
        SnapsEngine.SpeakString("sat on");
        M2();
    }

    public void StartProgram()
    {
        M1();
    }
}
