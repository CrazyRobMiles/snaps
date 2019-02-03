using SnapsLibrary;

class Ch11_01_PublicMenace
{
    class Contact
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

    public void StartProgram()
    {
        Contact insecure = new Contact("Rob", "Rob's House", "Rob's Phone");
        insecure.ContactMinutesSpent = -99;
        SnapsEngine.DisplayString("Minutes are " + insecure.ContactMinutesSpent);
    }
}