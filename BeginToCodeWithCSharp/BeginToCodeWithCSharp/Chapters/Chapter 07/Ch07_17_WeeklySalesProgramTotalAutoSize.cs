using SnapsLibrary;

class Ch07_17_WeeklySalesProgramTotalAutoSize
{
    public void StartProgram()
    {
        int[,] weeklySales = new int[7, 10];

        bool testMode = true;

        for (int day = 0; day < weeklySales.GetLength(0); day = day + 1)
        {
            for (int stand = 0; stand < weeklySales.GetLength(1); stand = stand + 1)
            {
                if (testMode)
                    weeklySales[day, stand] = day;
                else
                {
                    // User likes to count from 1, not zero
                    int displayCount = stand + 1;
                    weeklySales[day, stand] =
                        SnapsEngine.ReadInteger("Enter the sales for stand " + displayCount);
                }
            }
        }

        int totalSales = 0;
        for (int day = 0; day < 7; day = day + 1)
        {
            for (int stand = 0; stand < 10; stand = stand + 1)
            {
                totalSales = totalSales + weeklySales[day, stand];
            }
        }

        SnapsEngine.DisplayString("Total sales for the week are: " + totalSales);
    }
}
