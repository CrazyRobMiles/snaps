using SnapsLibrary;

class Ch03_06_TenSecondTimer
{
    public void StartProgram()
    {
        SnapsEngine.DisplayString("Start");
        SnapsEngine.Delay(10);
        SnapsEngine.DisplayString("End");
    }
}