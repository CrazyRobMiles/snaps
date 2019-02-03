using SnapsLibrary;
using System.Collections.Generic;

using Newtonsoft.Json;

public class Ch10_06_TimeTrackerJson
{
    class Contact
    {
        public string ContactName;
        public string ContactAddress;
        public string ContactPhone;

        public int MinutesSpent;

        public Contact(string name, string address, string phone)
        {
            this.ContactName = name;
            this.ContactAddress = address;
            this.ContactPhone = phone;
            this.MinutesSpent = 0;
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
            newContact.MinutesSpent = minutes;
            minutes = minutes + 30;
            storeContact(newContact);
        }
    }

    string SAVE_NAME = "TimeTracker.json";

    List<Contact> contacts = new List<Contact>();

    void storeContact(Contact contact)
    {
        contacts.Add(contact);
        storeAllContacts();
    }

    void storeAllContacts()
    {
        string json = JsonConvert.SerializeObject(contacts);

        SnapsEngine.SaveStringToLocalStorage(itemName: SAVE_NAME, itemValue: json);
    }

    void loadAllContacts()
    {
        string json = SnapsEngine.FetchStringFromLocalStorage(SAVE_NAME);

        if (json == null)
        {
            // If we get here there is no string in local storage
            SnapsEngine.WaitForButton("Created empty TimeTracker store");
            contacts = new List<Contact>();
        }
        else
        {
            contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
        }
    }

    void newContact()
    {
        SnapsEngine.SetTitleString("New Contact");
        string name = SnapsEngine.ReadString("Enter new contact name");
        string address = SnapsEngine.ReadMultiLineString("Enter contact address");
        string phone = SnapsEngine.ReadString("Enter contact phone");

        Contact newContact = new Contact(name: name, address: address, phone: phone);

        storeContact(newContact);

        storeAllContacts();

        SnapsEngine.DisplayString("Contact stored");

    }

    void findContact()
    {
        SnapsEngine.SetTitleString("Find Contact");

        string name = SnapsEngine.ReadString("Enter contact name");

        string resultString = "Contact not found";

        foreach (Contact contact in contacts)
        {
            if (contact.ContactName == name)
            {
                resultString =
                    "Name: " + contact.ContactName + "\n" +
                    "Address: " + contact.ContactAddress + "\n" +
                    "Phone: " + contact.ContactPhone + "\n" +
                    "Minutes: " + contact.MinutesSpent.ToString();
                break;
            }
        }
        SnapsEngine.DisplayString(resultString);
        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.DisplayString("");
    }

    void addMinutes()
    {
        SnapsEngine.SetTitleString("Add Minutes");

        string name = SnapsEngine.ReadString("Enter contact name");
        int minutes = SnapsEngine.ReadInteger("Enter contact minutes");

        string resultString = "Contact not found";

        for (int position = 0; position < contacts.Count; position = position + 1)
        {
            if (contacts[position].ContactName == name)
            {
                resultString = "Added " + minutes + " minutes\n" +
                    "to " + name;
                contacts[position].MinutesSpent = contacts[position].MinutesSpent + minutes;
                break;
            }
        }

        storeAllContacts();

        SnapsEngine.DisplayString(resultString);
        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.DisplayString("");
    }

    void displaySummary()
    {
        SnapsEngine.SetTitleString("Display Summary");

        for (int pass = 0; pass < contacts.Count - 1; pass = pass + 1)
        {
            for (int i = 0; i < contacts.Count - 1; i = i + 1)
            {
                if (contacts[i].MinutesSpent < contacts[i + 1].MinutesSpent)
                {
                    // the elements are in the wrong order, need to swap them round
                    Contact temp = contacts[i];
                    contacts[i] = contacts[i + 1];
                    contacts[i + 1] = temp;
                }
            }
        }

        string result = "";
        int listLimit;

        if (contacts.Count < 5)
            listLimit = contacts.Count;
        else
            listLimit = 5;

        for (int position = 0; position < listLimit; position = position + 1)
        {
            if (contacts[position].ContactName == null)
                continue;
            result = result + contacts[position].ContactName +
                ":" + contacts[position].MinutesSpent + "\n";
        }

        SnapsEngine.SetTitleString("Contact Times");

        SnapsEngine.DisplayString(result);

        SnapsEngine.WaitForButton("Continue");

        SnapsEngine.DisplayString("");
    }

    public void StartProgram()
    {
        // Remove this statement before you sell the program 
        // makeTestData();

        storeAllContacts();

        loadAllContacts();

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
