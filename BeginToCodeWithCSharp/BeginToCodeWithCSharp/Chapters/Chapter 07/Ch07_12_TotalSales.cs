using SnapsLibrary;

class Ch07_12_TotalSales
{
    public void StartProgram()
    {
        int[] sales = new int[]
        {
            50,54,29,33,22,100,45,54,89,75
        };

        SnapsEngine.SetTitleString("Highest and Lowest");

        int highest = sales[0];
        int lowest = sales[0];
        int total = 0;
        foreach (int sale in sales)
        {
            total = total + sale;
            if (sale < lowest)
                lowest = sale;
            if (sale > highest)
                highest = sale;
        }

        // Got the sames figures, now display them

        // start with an empty string
        string displaySummary;

        displaySummary = "Highest " + highest + "\n" +
            "Lowest " + lowest + "\n" +
            "Total " + total;

        // display the string
        SnapsEngine.DisplayString(displaySummary);
    }
}
