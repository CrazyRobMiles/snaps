
using SnapsLibrary;
using System.Collections.Generic;

class Ch07_18_DayNames
{
    public void StartProgram()
    {
        string[] dayNames = new string[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };

        int[,] weeklySales = new int[7, 10];

for (int day = 0; day < 7; day = day + 1)
{
    // Create of a list of sales for this day
    List<int> daySales = new List<int>();
    for (int stand = 0; stand < 10; stand = stand + 1)
    {
        // User likes to count from 1, not zero
        int displayCount = stand + 1;
        weeklySales[stand, day] =
            SnapsEngine.ReadInteger("Enter the sales for stand " + 
                                displayCount+ " on " + dayNames[day]);
    } 
}
    }
}

