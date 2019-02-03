using SnapsLibrary;

class Ch07_13_CompleteProgram
{
    public void StartProgram()
    {
        while (true)
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

            while (true)
            {
                string command = SnapsEngine.SelectFrom6Buttons(
                    "Low to High",
                    "High to Low",
                    "Highest and Lowest",
                    "Total sales",
                    "Average sales",
                    "Enter figures");

                if (command == "Low to High")
                {
                    SnapsEngine.SetTitleString("Low to High Sales");

                    for (int pass = 0; pass < sales.Length - 1; pass = pass + 1)
                    {
                        // clear the swap flag for this pass
                        bool doneSwap = false;

                        // Make a pass down the array swapping elements
                        for (int i = 0; i < sales.Length - 1; i = i + 1)
                        {
                            if (sales[i] > sales[i + 1])
                            {
                                // the elements are in the wrong order, need to swap them round
                                int temp = sales[i];
                                sales[i] = sales[i + 1];
                                sales[i + 1] = temp;
                                doneSwap = true;
                            }
                        }
                        if (!doneSwap)
                            // quit the sort if we didn't do any swaps
                            break;
                    }

                    // Now print out the sorted data

                    SnapsEngine.ClearTextDisplay();

                    // Add a line to the string for each sales figure
                    for (int count = 0; count < sales.Length; count = count + 1)
                    {
                        SnapsEngine.AddLineToTextDisplay("Sales: " + sales[count]);
                    }

                    SnapsEngine.WaitForButton("Continue");
                    SnapsEngine.DisplayString("");
                }


                if (command == "High to Low")
                {
                    SnapsEngine.SetTitleString("High to Low Sales");

                    for (int pass = 0; pass < sales.Length - 1; pass = pass + 1)
                    {
                        // clear the swap flag for this pass
                        bool doneSwap = false;

                        // Make a pass down the array swapping elements
                        for (int i = 0; i < sales.Length - 1; i = i + 1)
                        {
                            if (sales[i] < sales[i + 1])
                            {
                                // the elements are in the wrong order, need to swap them round
                                int temp = sales[i];
                                sales[i] = sales[i + 1];
                                sales[i + 1] = temp;
                                doneSwap = true;
                            }
                        }
                        if (!doneSwap)
                            // quit the sort if we didn't do any swaps
                            break;
                    }

                    // Now print out the sorted data

                    SnapsEngine.ClearTextDisplay();

                    // Add a line to the string for each sales figure
                    for (int count = 0; count < sales.Length; count = count + 1)
                    {
                        SnapsEngine.AddLineToTextDisplay("Sales: " + sales[count]);
                    }

                    SnapsEngine.WaitForButton("Continue");
                    SnapsEngine.DisplayString("");
                }

                if (command == "Highest and Lowest")
                {
                    SnapsEngine.SetTitleString("Highest and Lowest");

                    int highest = sales[0];
                    int lowest = sales[0];
                    foreach (int sale in sales)
                    {
                        if (sale > highest)
                            highest = sale;
                        if (sale < lowest)
                            lowest = sale;
                    }

                    SnapsEngine.DisplayString("Highest " + highest + "\n" +
                        "Lowest " + lowest);

                    SnapsEngine.WaitForButton("Continue");
                    SnapsEngine.DisplayString("");
                }

                if (command == "Total sales")
                {
                    SnapsEngine.SetTitleString("Total Sales");

                    int totalSales = 0;
                    foreach (int sale in sales)
                        totalSales = totalSales + sale;

                    SnapsEngine.DisplayString("Total sales " + totalSales);

                    SnapsEngine.WaitForButton("Continue");
                    SnapsEngine.DisplayString("");
                }

                if (command == "Average sales")
                {
                    SnapsEngine.SetTitleString("Average sales");

                    int total = 0;
                    foreach (int sale in sales)
                        total = total + sale;

                    float average = (float)total / sales.Length;

                    SnapsEngine.DisplayString("Average sales " + average);

                    SnapsEngine.WaitForButton("Continue");
                    SnapsEngine.DisplayString("");
                }

                if (command == "Enter figures")
                {
                    SnapsEngine.SetTitleString("Enter New Data");

                    string confirm = SnapsEngine.SelectFrom2Buttons("Enter new data", "Cancel");

                    if (confirm == "Enter new data")
                    {
                        break;
                    }
                }
            }
        }
    }
}
