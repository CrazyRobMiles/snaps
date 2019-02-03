using SnapsLibrary;

class Ch09_14_DrawingClear
{
    public void StartProgram()
    {
        while (true)
        {
            SnapsCoordinate drawPos = SnapsEngine.GetDraggedCoordinate();
            if (drawPos.XValue < 10 && drawPos.YValue < 10)
                SnapsEngine.ClearGraphics();
            else
                SnapsEngine.DrawDot(pos: drawPos, width: 20);
        }
    }
}