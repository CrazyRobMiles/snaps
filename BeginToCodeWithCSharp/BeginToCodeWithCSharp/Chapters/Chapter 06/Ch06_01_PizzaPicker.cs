using SnapsLibrary;

class Ch06_01_PizzaPicker
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
            string toppingChoice = SnapsEngine.SelectFrom5Buttons(
                "Cheese and Tomato",
                "Pepperoni",
                "Chicken",
                "Vegetarian",
                "Show Totals");

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
                SnapsEngine.ClearTextDisplay();

                SnapsEngine.AddLineToTextDisplay("Order Totals");
                SnapsEngine.AddLineToTextDisplay(cheeseAndTomatoCount.ToString() + " Cheese and Tomato");
                SnapsEngine.AddLineToTextDisplay(pepperoniCount.ToString() + " Pepperoni");
                SnapsEngine.AddLineToTextDisplay(chickenCount.ToString() + " Chicken");
                SnapsEngine.AddLineToTextDisplay(vegetarianCount.ToString() + " Vegetarian");

                string reply = SnapsEngine.SelectFrom2Buttons(item1: "Done", item2: "Reset");

                if (reply == "Reset")
                {
                    cheeseAndTomatoCount = 0;
                    pepperoniCount = 0;
                    chickenCount = 0;
                    vegetarianCount = 0;
                }

                // clear the total display from the screen ready for more choices
                SnapsEngine.ClearTextDisplay();
            }
        }
    }
}