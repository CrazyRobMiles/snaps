using SnapsLibrary;

class Ch07_02_ArrayExceptions
{
    public void StartProgram()
    {
        int[] sales = new int[10];
        sales[0] = 99;
        // this statement will throw an exception
        sales[10] = 50;
    }
}
