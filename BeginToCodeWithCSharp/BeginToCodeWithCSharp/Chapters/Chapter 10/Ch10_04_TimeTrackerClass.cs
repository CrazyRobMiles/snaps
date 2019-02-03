using SnapsLibrary;
using System.Collections.Generic;

class Ch10_04_TimeTrackerClass
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


    void makeTestData()
    {
        string[] testNames = {
            "Rob", "Mary", "David", "Jenny",
            "Simon", "Kevin", "Helen", "Chris",
            "Amanda", "Sally" };

        // the number of minutes for contacts
        int minutes = 0;

        foreach (string name in testNames)
        {
            Contact newContact = new Contact(name: name,
                address: name + "'s house",
                phone: name + "'s phone");
            newContact.ContactMinutesSpent = minutes;
            minutes = minutes + 30;
            storeContact(newContact);
        }
    }

    List<Contact> contacts = new List<Contact>();

    void storeContact(Contact contact)
    {
        contacts.Add(contact);
    }

    void newContact()
    {
        SnapsEngine.SetTitleString("New Contact");
        string name = SnapsEngine.ReadString("Enter new contact name");
        string address = SnapsEngine.ReadMultiLineString("Enter contact address");
        string phone = SnapsEngine.ReadString("Enter contact phone");

        Contact newContact = new Contact(name: name, address: address, phone: phone);

        storeContact(newContact);

        SnapsEngine.DisplayString("Contact stored");
    }

    void findContact()
    {
        SnapsEngine.SetTitleString("Find Contact");

        string name = SnapsEngine.ReadString("Enter contact name");

        bool foundAContact = false;

        SnapsEngine.ClearTextDisplay();

        foreach (Contact contact in contacts)
        {
            if (contact.ContactName == name)
            {
                SnapsEngine.AddLineToTextDisplay("Name: " + contact.ContactName);
                SnapsEngine.AddLineToTextDisplay("Address: " + contact.ContactAddress);
                SnapsEngine.AddLineToTextDisplay("Phone: " + contact.ContactPhone);
                SnapsEngine.AddLineToTextDisplay("Minutes: " + contact.ContactMinutesSpent.ToString());
                foundAContact = true;
                break;
            }
        }

        if (!foundAContact)
            SnapsEngine.AddLineToTextDisplay("Contact not found");

        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.ClearTextDisplay();
    }

    void addMinutes()
    {
        SnapsEngine.SetTitleString("Add Minutes");

        string name = SnapsEngine.ReadString("Enter contact name");
        int minutes = SnapsEngine.ReadInteger("Enter contact minutes");

        bool foundAContact = false;

        SnapsEngine.ClearTextDisplay();

        for (int position = 0; position < contacts.Count; position = position + 1)
        {
            if (contacts[position].ContactName == name)
            {
                SnapsEngine.AddLineToTextDisplay("Added " + minutes + " minutes\n" +
                    "to " + name);
                contacts[position].ContactMinutesSpent = contacts[position].ContactMinutesSpent + minutes;
                foundAContact = true;
                break;
            }
        }

        if (!foundAContact)
            SnapsEngine.AddLineToTextDisplay("Contact not found");

        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.ClearTextDisplay();
    }

    void displaySummary()
    {
        SnapsEngine.SetTitleString("Display Summary");

        for (int pass = 0; pass < contacts.Count - 1; pass = pass + 1)
        {
            for (int i = 0; i < contacts.Count - 1; i = i + 1)
            {
                if (contacts[i].ContactMinutesSpent < contacts[i + 1].ContactMinutesSpent)
                {
                    // the elements are in the wrong order, need to swap them round
                    Contact temp = contacts[i];
                    contacts[i] = contacts[i + 1];
                    contacts[i + 1] = temp;
                }
            }
        }

        SnapsEngine.SetTitleString("Contact Times");

        SnapsEngine.ClearTextDisplay();

        for (int position = 0; position < 5; position = position + 1)
        {
            if (contacts[position].ContactName == null)
                break;
            SnapsEngine.AddLineToTextDisplay(contacts[position].ContactName +
                ":" + contacts[position].ContactMinutesSpent);
        }

        SnapsEngine.WaitForButton("Continue");

        SnapsEngine.ClearTextDisplay();
    }

    public void StartProgram()
    {
        // Remove this statement before you sell the program 
        makeTestData();

        while (true)
        {
            SnapsEngine.SetTitleString("Time Tracker");

            string command = SnapsEngine.SelectFrom4Buttons("New Contact", "Find Contact",
                "Add Minutes", "Display Summary");

            switch (command)
            {
                case "New Contact":
                    newContact();
                    break;

                case "Find Contact":
                    findContact();
                    break;

                case "Add Minutes":
                    addMinutes();
                    break;

                case "Display Summary":
                    displaySummary();
                    break;
            }
        }
    }
}
