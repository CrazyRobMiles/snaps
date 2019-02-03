using SnapsLibrary;

class Ch07_05_BadlyNumberedStand
{
    public void StartProgram()
    {
        int noOfStands = SnapsEngine.ReadInteger("How many ice cream stands");
        int[] sales = new int[noOfStands];

        for (int count = 0; count < sales.Length; count = count + 1)
        {
            sales[count] = SnapsEngine.ReadInteger("Enter the sales for stand " + count + 1);
        }
    }
}