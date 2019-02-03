using SnapsLibrary;

class Ch09_17_DrawLineDemo
{
    public void StartProgram()
    {
        SnapsEngine.SetDrawingColor(red: 255, green: 0, blue: 0);
        SnapsEngine.DrawLine(x1: 0, y1: 0, x2: 100, y2: 100);
        SnapsEngine.SetDrawingColor(red: 0, green: 0, blue: 255);
        SnapsEngine.DrawLine(x1: 0, y1: 100, x2: 100, y2: 0);
    }
}