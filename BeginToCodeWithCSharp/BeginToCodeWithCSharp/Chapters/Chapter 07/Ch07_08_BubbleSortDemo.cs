using SnapsLibrary;

class Ch07_08_BubbleSortDemo
{
    public void StartProgram()
    {
        // To save us from typing in the values, I've pre-set them into an array 
        // of integers. 
        int[] sales = new int[]
        {
            50,54,29,33,22,100,45,54,89,75
        };

        SnapsEngine.SetTitleString("Sort Demo");

        // Single pass
        for (int count = 0; count < sales.Length - 1; count = count + 1)
        {
            if (sales[count] > sales[count + 1])
            {
                // the elements are in the wrong order, need to swap them round
                int temp = sales[count];
                sales[count] = sales[count + 1];
                sales[count + 1] = temp;
            }
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
