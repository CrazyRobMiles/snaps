using SnapsLibrary;

class Ch10_03_CupCakeStructureWithThis
{
    struct CupCake
    {
        public string Name;
        public string Ingredients;
        public string Recipe;

        public CupCake(string Name, string Ingredients, string Recipe)
        {
            this.Name = Name;
            this.Ingredients = Ingredients;
            this.Recipe = Recipe;
        }
    }

    public static void StartProgram()
    {
        CupCake cheeseSurprise;
        cheeseSurprise.Name = "Cheese Surprise";
        cheeseSurprise.Ingredients = "Cheese, flour, sugar";
        cheeseSurprise.Recipe = "Mix cheese, flour and sugar, cook for 30 minutes";

        SnapsEngine.SetTitleString("CupCake Structure Demo");

        SnapsEngine.ClearTextDisplay();
        SnapsEngine.AddLineToTextDisplay("Name: " + cheeseSurprise.Name);
        SnapsEngine.AddLineToTextDisplay("Ingredients: " + cheeseSurprise.Ingredients);
        SnapsEngine.AddLineToTextDisplay("Recipe: " + cheeseSurprise.Recipe);

    }

}

