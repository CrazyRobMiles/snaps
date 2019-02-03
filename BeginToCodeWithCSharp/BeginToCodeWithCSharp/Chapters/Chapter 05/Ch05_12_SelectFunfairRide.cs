using SnapsLibrary;

class Ch05_12_SelectFunfairRide
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
    }
}