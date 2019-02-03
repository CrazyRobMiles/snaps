using SnapsLibrary;

class Ch09_12_MysteryImage
{
    public void StartProgram()
    {
        SnapsEngine.SetBackgroundColor(red: 100, green: 100, blue: 100);
        SnapsCoordinate pos = new SnapsCoordinate(100, 200);
        SnapsEngine.SetDrawingColor(red: 255, green: 255, blue: 255);
        SnapsEngine.DrawDot(pos: pos, width: 100);
        SnapsEngine.SetDrawingColor(red: 0, green: 0, blue: 0);
        SnapsEngine.DrawDot(pos: pos, width: 80);
        SnapsEngine.SetDrawingColor(red: 0, green: 0, blue: 255);
        SnapsEngine.DrawDot(pos: pos, width: 60);
        SnapsEngine.SetDrawingColor(red: 255, green: 0, blue: 0);
        SnapsEngine.DrawDot(pos: pos, width: 40);
        SnapsEngine.SetDrawingColor(red: 255, green: 255, blue: 0);
        SnapsEngine.DrawDot(pos: pos, width: 20);
    }
}