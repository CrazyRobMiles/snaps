using SnapsLibrary;

class Ch07_07_ReadAndDisplay
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Ice Cream Sales");

        // Find out how many sales values are being stored
        int noOfStands = SnapsEngine.ReadInteger("How many ice cream stands");
        int[] sales = new int[noOfStands];

        // Loop round and read the sales values
        for (int count = 0; count < sales.Length; count = count + 1)
        {
            // User likes to count from 1, not zero
            int displayCount = count + 1;
            sales[count] = SnapsEngine.ReadInteger("Enter the sales for stand " + displayCount);
        }

        // Got the sales figures, now display them

        SnapsEngine.ClearTextDisplay();

        // Add a line to the display for each sales figure
        for (int count = 0; count < sales.Length; count = count + 1)
        {
            SnapsEngine.AddLineToTextDisplay("Sales: " + sales[count]);
        }
    }
}
