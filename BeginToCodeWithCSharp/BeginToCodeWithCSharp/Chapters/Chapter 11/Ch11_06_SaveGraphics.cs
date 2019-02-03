using SnapsLibrary;

class Ch11_06_SaveGraphics
{
    public void StartProgram()
    {
        SnapsEngine.DrawDot(x: 100, y: 100, width: 50);

        SnapsEngine.SaveGraphicsImageToFileAsPNG();
    }
}
