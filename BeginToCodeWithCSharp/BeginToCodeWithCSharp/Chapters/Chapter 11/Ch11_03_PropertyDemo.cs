using SnapsLibrary;

class Ch11_03_PropertyDemo
{
    class Contact
    {
        private string contactName;

        public string ContactName
        {
            get
            {
                SnapsEngine.DisplayString("Getting the value of the name");
                return this.contactName;
            }

            set
            {
                SnapsEngine.DisplayString("Setting the name to " + value);
                this.contactName = value;
            }
        }
    }

    public static void StartProgram()
    {
        SnapsEngine.SetTitleString("Name Property Demo");

        Contact rob = new Contact();
        rob.ContactName = "Robert";
        SnapsEngine.WaitForButton("Continue");
        string name = rob.ContactName;
        SnapsEngine.WaitForButton("Continue");
    }
}
