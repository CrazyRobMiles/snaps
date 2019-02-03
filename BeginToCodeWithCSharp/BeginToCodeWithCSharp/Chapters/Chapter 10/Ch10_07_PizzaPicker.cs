using SnapsLibrary;

using Newtonsoft.Json;

class Ch10_07_PizzaPicker
{
    class PizzaDetails
    {
        public int CheeseAndTomatoCount = 0;
        public int pepperoniCount = 0;
        public int chickenCount = 0;
        public int vegetarianCount = 0;
    }

    public void StartProgram()
    {
        string SAVE_NAME = "pizzaChoice.json";

        PizzaDetails pizzaDetails;

        string json = SnapsEngine.FetchStringFromRoamingStorage(SAVE_NAME);

        if (json == null)
        {
            // No stored pizza details - make an empty one
            pizzaDetails = new PizzaDetails();
        }
        else
        {
            // Read the pizza counts from last time
            pizzaDetails = JsonConvert.DeserializeObject<PizzaDetails>(json);
        }

        SnapsEngine.SetTitleString("Select Pizza");

        // repeatedly ask for pizza selections
        while (true)
        {
            string toppingChoice = SnapsEngine.SelectFrom5Buttons("Cheese and Tomato",
                                                            "Pepperoni",
                                                            "Chicken", "Vegetarian",
                                                            "Show Totals");

            if (toppingChoice == "Cheese and Tomato")
                pizzaDetails.CheeseAndTomatoCount = pizzaDetails.CheeseAndTomatoCount + 1;

            if (toppingChoice == "Pepperoni")
                pizzaDetails.pepperoniCount = pizzaDetails.pepperoniCount + 1;

            if (toppingChoice == "Chicken")
                pizzaDetails.chickenCount = pizzaDetails.chickenCount + 1;

            if (toppingChoice == "Vegetarian")
                pizzaDetails.vegetarianCount = pizzaDetails.vegetarianCount + 1;

            if (toppingChoice == "Show Totals")
            {
                string result = "Order Totals:\n" +
                    pizzaDetails.CheeseAndTomatoCount.ToString() + " Cheese and Tomato\n" +
                    pizzaDetails.pepperoniCount.ToString() + " Pepperoni\n" +
                    pizzaDetails.chickenCount.ToString() + " Chicken\n" +
                    pizzaDetails.vegetarianCount.ToString() + " Vegetarian\n";

                SnapsEngine.DisplayString(result);

                string reply = SnapsEngine.SelectFrom2Buttons("Done", "Reset");
                if (reply == "Reset")
                {
                    pizzaDetails.CheeseAndTomatoCount = 0;
                    pizzaDetails.pepperoniCount = 0;
                    pizzaDetails.chickenCount = 0;
                    pizzaDetails.vegetarianCount = 0;
                }
                // clear the total display from the screen
                SnapsEngine.DisplayString("");
            }

json = JsonConvert.SerializeObject(pizzaDetails);

SnapsEngine.SaveStringToRoamingStorage(itemName: SAVE_NAME, itemValue: json);
        }
    }
}