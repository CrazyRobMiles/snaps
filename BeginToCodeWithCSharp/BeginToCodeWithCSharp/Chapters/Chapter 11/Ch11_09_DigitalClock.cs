using SnapsLibrary;
using System;

class Ch11_09_DigitalClock
{
    public void StartProgram()
    {
        

        SnapsEngine.SetTitleString("Snaps Clock");

        while(true)
        {
            DateTime currentDateAndTime = DateTime.Now;

            SnapsEngine.DisplayString(currentDateAndTime.ToString());

            SnapsEngine.Delay(1);
        }
    }
}
