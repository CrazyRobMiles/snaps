using SnapsLibrary;

class Ch11_08_LoadGraphicsLocal
{
    public void StartProgram()
    {
        

        if (!SnapsEngine.LoadGraphicsPNGImageFromLocalStore("test.png"))
        {
            SnapsEngine.DisplayDialog("image not found");
        }
    }
}
