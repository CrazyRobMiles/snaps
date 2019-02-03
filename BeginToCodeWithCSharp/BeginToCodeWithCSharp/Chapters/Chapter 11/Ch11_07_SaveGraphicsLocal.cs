using SnapsLibrary;

class Ch11_07_SaveGraphicsLocal
{
    public void StartProgram()
    {
        

        SnapsEngine.DrawDot(x: 100, y: 100, width: 50);

        SnapsEngine.SaveGraphicsImageToLocalStoreAsPNG("test.png");
    }
}
