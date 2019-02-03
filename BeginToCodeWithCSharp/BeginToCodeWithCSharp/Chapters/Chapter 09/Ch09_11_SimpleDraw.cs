using SnapsLibrary;

class Ch09_11_SimpleDraw
{
    public void StartProgram()
    {
        while (true)
        {
            SnapsCoordinate drawPos = SnapsEngine.GetDraggedCoordinate();
            SnapsEngine.DrawDot(pos: drawPos, width: 10);
        }
    }
}