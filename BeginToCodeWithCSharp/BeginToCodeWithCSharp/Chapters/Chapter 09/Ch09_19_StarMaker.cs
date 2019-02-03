using SnapsLibrary;

class Ch09_19_StarMaker
{
    public void StartProgram()
    {
        SnapsCoordinate screenSize = SnapsEngine.GetScreenSize();
        SnapsCoordinate center;
        center.XValue = screenSize.XValue / 2;
        center.YValue = screenSize.YValue / 2;

        SnapsEngine.SetDrawingColor(red: 255, green: 0, blue: 0);

        while (true)
        {
            SnapsCoordinate lineEnd = SnapsEngine.GetTappedCoordinate();
            SnapsEngine.DrawLine(center, lineEnd);
        }
    }
}