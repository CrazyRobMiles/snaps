using SnapsLibrary;

class Ch07_04_VariableArraySizes
{
    public void StartProgram()
    {
        int noOfStands = SnapsEngine.ReadInteger("How many ice cream stands");
        int[] sales = new int[noOfStands];

        for (int count = 0; count < sales.Length; count = count + 1)
        {
            sales[count] = SnapsEngine.ReadInteger("Enter the sales value");
        }
    }
}

