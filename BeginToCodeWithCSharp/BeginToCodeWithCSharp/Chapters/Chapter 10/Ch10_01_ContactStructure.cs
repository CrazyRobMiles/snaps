using SnapsLibrary;

class Ch10_01_ContactStructure
{
    struct Contact
    {
        public string ContactName;
        public string ContactAddress;
        public string ContactPhone;

        public int ContactMinutesSpent;

        public Contact(string name, string address, string phone)
        {
            this.ContactName = name;
            this.ContactAddress = address;
            this.ContactPhone = phone;
            this.ContactMinutesSpent = 0;
        }
    }

public static void StartProgram()
{
    Contact rob = new Contact(name: "Rob", address: "Rob's House", phone: "Rob's Phone");

    SnapsEngine.SetTitleString("Contact Structure Demo");

    SnapsEngine.ClearTextDisplay();
    SnapsEngine.AddLineToTextDisplay("Name: " + rob.ContactName);
    SnapsEngine.AddLineToTextDisplay("Address: " + rob.ContactAddress);
    SnapsEngine.AddLineToTextDisplay("Phone: " + rob.ContactPhone);
    SnapsEngine.AddLineToTextDisplay("Minutes: " + rob.ContactMinutesSpent.ToString());
}
}
 