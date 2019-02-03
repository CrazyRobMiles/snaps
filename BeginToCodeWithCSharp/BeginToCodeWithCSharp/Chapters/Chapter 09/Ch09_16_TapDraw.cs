using SnapsLibrary;

class Ch09_16_TapDraw
{
    public void StartProgram()
    {
        while (true)
        {
            SnapsCoordinate tappedPos = SnapsEngine.GetTappedCoordinate();
            SnapsEngine.DrawDot(tappedPos, 20);
        }
    }
}
