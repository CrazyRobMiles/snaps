using SnapsLibrary;

class Ch05_15_CompleteFunfairProgram
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
        else
        {
            // We need to read the age of the user
            int ageInt = SnapsEngine.ReadInteger("What is your age");

            if (ride == "Carnival Carousel")
            {
                if (ageInt >= 3)
                    SnapsEngine.DisplayString("You can go on the ride.");
                else
                    SnapsEngine.DisplayString("I'm sorry. You are too young.");
            }

            if (ride == "Jungle Adventure Water Splash")
            {
                if (ageInt >= 6)
                    SnapsEngine.DisplayString("You can go on the ride.");
                else
                    SnapsEngine.DisplayString("I'm sorry. You are too young.");
            }

            if (ride == "Downhill Mountain Run")
            {
                if (ageInt >= 12)
                    SnapsEngine.DisplayString("You can go on the ride.");
                else
                    SnapsEngine.DisplayString("I'm sorry. You are too young.");
            }

            if (ride == "The Regurgitator")
            {
                // If we get here we are dealing with The Regurgitator
                if (ageInt >= 12)
                {
                    // If we get here the age is not too low
                    if (ageInt > 70)
                    {
                        // If we get here the age is too high
                        SnapsEngine.DisplayString("I'm sorry. You are too old.");
                    }
                    else
                    {
                        // If we get here the age is in the correct range
                        SnapsEngine.DisplayString("You can go on the ride");
                    }
                }
                else
                {
                    // If we get here the age is too low
                    SnapsEngine.DisplayString("I'm sorry. You are too young.");
                }
            }
        }
    }
}
