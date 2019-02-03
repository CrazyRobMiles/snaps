using SnapsLibrary;

class Ch07_09_BubbleSortWorking
{
    public void StartProgram()
    {
        int[] sales = new int[]
        {
            50,54,29,33,22,100,45,54,89,75
        };

        SnapsEngine.SetTitleString("Sort Demo");

        for (int pass = 0; pass < sales.Length - 1; pass = pass + 1)
        {
            for (int i = 0; i < sales.Length - 1; i = i + 1)
            {
                if (sales[i] > sales[i + 1])
                {
                    // the elements are in the wrong order, need to swap them round
                    int temp = sales[i];
                    sales[i] = sales[i + 1];
                    sales[i + 1] = temp;
                }
            }
        }

        // Got the sorted figures, now display them

        SnapsEngine.ClearTextDisplay();

        // Add a line to the display for each sales figure
        for (int count = 0; count < sales.Length; count = count + 1)
        {
            SnapsEngine.AddLineToTextDisplay("Sales: " + sales[count]);
        }
    }
}
