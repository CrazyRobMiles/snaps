using SnapsLibrary;

class Ch05_13_HandleRiverCruise
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Super Funfair Rides");
        string ride;
        ride = SnapsEngine.SelectFrom5Buttons(
            "Scenic River Cruise",
            "Carnival Carousel",
            "Jungle Adventure Water Splash",
            "Downhill Mountain Run",
            "The Regurgitator");

        SnapsEngine.SetTitleString(ride);

        if (ride == "Scenic River Cruise")
        {
            SnapsEngine.DisplayString("There are no age restrictions on this ride. Enjoy.");
        }
    }
}
