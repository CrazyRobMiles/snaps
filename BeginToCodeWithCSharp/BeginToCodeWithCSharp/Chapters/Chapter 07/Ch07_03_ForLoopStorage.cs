using SnapsLibrary;

class Ch07_03_ForLoopStorage
{
    public void StartProgram()
    {
        int[] sales = new int[10];
        for (int count = 0; count < 10; count = count + 1)
        {
            sales[count] = SnapsEngine.ReadInteger("Enter the sales value");
        }
    }
}
