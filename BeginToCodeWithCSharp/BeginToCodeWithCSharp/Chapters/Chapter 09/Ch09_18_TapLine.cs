using SnapsLibrary;

class Ch09_18_TapLine
{
    public void StartProgram()
    {
        SnapsCoordinate origin = new SnapsCoordinate(x: 0, y: 0);

        SnapsEngine.SetDrawingColor(red: 255, green: 0, blue: 0);
        while (true)
        {
            SnapsCoordinate lineEnd = SnapsEngine.GetTappedCoordinate();
            SnapsEngine.DrawLine(p1: origin, p2: lineEnd);
        }
    }
}
