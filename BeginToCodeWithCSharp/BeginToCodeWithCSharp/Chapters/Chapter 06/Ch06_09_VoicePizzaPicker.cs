using SnapsLibrary;

class Ch06_09_VoicePizzaPicker
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Select Pizza");

        int cheeseAndTomatoCount = 0;
        int pepperoniCount = 0;
        int chickenCount = 0;
        int vegetarianCount = 0;

        // repeatedly ask for pizza selections

        while (true)
        {

            SnapsEngine.SpeakString("Tell me your choice:");

            string toppingChoice = SnapsEngine.SelectFromFiveSpokenPhrases(
                    prompt: "What pizza topping do you want",
                    phrase1: "Cheese and Tomato",
                    phrase2: "Pepperoni",
                    phrase3: "Chicken",
                    phrase4: "Vegetarian",
                    phrase5: "Show Totals");

            if (toppingChoice == "")
            {
                SnapsEngine.SpeakString("Sorry, choice not recognised");
                continue;
            }

            SnapsEngine.DisplayString(toppingChoice);

            if (toppingChoice == "Cheese and Tomato")
                cheeseAndTomatoCount = cheeseAndTomatoCount + 1;

            if (toppingChoice == "Pepperoni")
                pepperoniCount = pepperoniCount + 1;

            if (toppingChoice == "Chicken")
                chickenCount = chickenCount + 1;

            if (toppingChoice == "Vegetarian")
                vegetarianCount = vegetarianCount + 1;

            if (toppingChoice == "Show Totals")
            {
                SnapsEngine.DisplayString("Order Totals");

                SnapsEngine.AddLineToTextDisplay(cheeseAndTomatoCount.ToString() + " Cheese and Tomato");
                SnapsEngine.AddLineToTextDisplay(pepperoniCount.ToString() + " Pepperoni");
                SnapsEngine.AddLineToTextDisplay(chickenCount.ToString() + " Chicken");
                SnapsEngine.AddLineToTextDisplay(vegetarianCount.ToString() + " Vegetarian");

                string reply = SnapsEngine.SelectFrom2Buttons("Done", "Reset");
                if (reply == "Reset")
                {
                    cheeseAndTomatoCount = 0;
                    pepperoniCount = 0;
                    chickenCount = 0;
                    vegetarianCount = 0;
                }
                // clear the total display from the screen
                SnapsEngine.ClearTextDisplay();
            }
        }
    }
}