using SnapsLibrary;
using System;

class Ch11_10_DigitalClockNoFade
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Snaps Clock");

        while(true)
        {
            DateTime currentDateAndTime = DateTime.Now;

            SnapsEngine.DisplayString(message:currentDateAndTime.ToString(), 
                alignment: SnapsTextAlignment.centre,
                fadeType: SnapsFadeType.nofade, 
                size: 50);

            SnapsEngine.Delay(1);
        }
    }
}
