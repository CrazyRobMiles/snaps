using SnapsLibrary;

using Newtonsoft.Json;

class Ch10_06_JsonDemo
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
        SnapsEngine.SetTitleString("Json Demo");

        string name = SnapsEngine.ReadString("Enter new contact name");
        string address = SnapsEngine.ReadMultiLineString("Enter contact address");
        string phone = SnapsEngine.ReadString("Enter contact phone");

        Contact newContact = new Contact(name: name, address: address, phone: phone);

        string json = JsonConvert.SerializeObject(newContact);

        SnapsEngine.DisplayString(json);

        SnapsEngine.WaitForButton("Press to de-serialise");

        Contact contact = JsonConvert.DeserializeObject<Contact>(json);

        string resultString =
            "Name: " + contact.ContactName + "\n" +
            "Address: " + contact.ContactAddress + "\n" +
            "Phone: " + contact.ContactPhone + "\n" +
            "Minutes: " + contact.ContactMinutesSpent.ToString();

        SnapsEngine.DisplayString(resultString);

    }
}
