using SnapsLibrary;
using System.Collections.Generic;

using Newtonsoft.Json;
using System;

class Ch11_04_CatchingExceptions
{


    class Contact
    {
        private string contactName;

        public static string ValidateName(string newName)
        {
            if (newName == "")
                return "A name cannot be an empty string\n";

            return "";
        }

        public string ContactName
        {
            get
            {
                return contactName;
            }

            set
            {
                string message = ValidateName(value);
                if (message != "")
                    throw new Exception(message);

                contactName = value;
            }
        }

        private string contactAddress;

        public static string ValidateAddress(string newAddress)
        {
            if (newAddress == "")
                return "An address cannot be an empty string\n";

            return "";
        }

        public string ContactAddress
        {
            get
            {
                return contactAddress;
            }

            set
            {
                contactAddress = value;
            }
        }

        private string contactPhone;

        public static string ValidatePhone(string newPhone)
        {
            if (newPhone == "")
                return "A phone number cannot be an empty string\n";

            return "";
        }

        public string ContactPhone
        {
            get
            {
                return contactPhone;
            }

            set
            {
                contactPhone = value;
            }
        }

        private int contactMinutesSpent;

        public static string ValidateMinutesSpent(int newMinutes)
        {
            if (newMinutes < 0)
                return "The minutes spent value must be greater than 0\n";

            return "";
        }

        public int ContactMinutesSpent
        {
            get
            {
                return contactMinutesSpent;
            }

            set
            {
                contactMinutesSpent = value;
            }
        }

        public Contact(string name, string address, string phone)
        {
            // errorMessage contains the complete error message
            string errorMessage = "";

            // error contains the message produced by each validation    
            string error;

            // validate the name
            error = ValidateName(name);
            // if the name is invalid the error string holds the reason
            if (error != "")
                // if we get here there is an error in the name
                errorMessage = error;

            // validate the address
            error = ValidateAddress(address);
            // if the address is invalid the error string holds the reason
            if (error != "")
                // if we get here there is an error in the address
                // add it to the error report
                errorMessage = errorMessage + error;

            // validate the phone number
            error = ValidatePhone(phone);
            // if the phone number is invalid the error string holds the reason
            if (error != "")
                // if we get here there is an error in the phone number
                // add it to the error report
                errorMessage = errorMessage + error;

            // if the error message is not an empty string something went wrong
            if (errorMessage != "")
                // Abandon construction by throwing an exception
                throw new Exception(errorMessage);

            this.ContactName = name;
            this.ContactAddress = address;
            this.ContactPhone = phone;
            this.contactMinutesSpent = 0;
        }
    }

    static void makeTestData()
    {
        string[] testNames = {
    "Rob", "Mary", "David", "Jenny",
    "Simon", "Kevin", "Helen", "Neil",
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

    static string SAVE_NAME = "TimeTracker1.json";

    static List<Contact> contacts = new List<Contact>();

    static void storeContact(Contact contact)
    {
        contacts.Add(contact);
        storeAllContacts();
    }

    static void storeAllContacts()
    {
        string json = JsonConvert.SerializeObject(contacts);

        SnapsEngine.SaveStringToLocalStorage(itemName: SAVE_NAME, itemValue: json);
    }

    static void loadAllContacts()
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

    static void newContact()
    {
        SnapsEngine.SetTitleString("New Contact");

        string name = SnapsEngine.ReadString("Enter new contact name");
        string address = SnapsEngine.ReadMultiLineString("Enter contact address");
        string phone = SnapsEngine.ReadString("Enter contact phone");

        Contact newContact;

        try
        {
            newContact = new Contact(name: name, address: address, phone: phone);
            storeContact(newContact);
            storeAllContacts();
            SnapsEngine.DisplayString("Contact stored");
        }
        catch (Exception e)
        {
            SnapsEngine.SetTitleString("Could not create contact");
            SnapsEngine.DisplayString(e.Message);
        }
        SnapsEngine.WaitForButton("Continue");
    }

    static void findContact()
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
                    "Minutes: " + contact.ContactMinutesSpent.ToString();
                break;
            }
        }
        SnapsEngine.DisplayString(resultString);
        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.DisplayString("");
    }

    static void addMinutes()
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
                contacts[position].ContactMinutesSpent = contacts[position].ContactMinutesSpent + minutes;
                break;
            }
        }

        storeAllContacts();

        SnapsEngine.DisplayString(resultString);
        SnapsEngine.WaitForButton("Continue");
        SnapsEngine.DisplayString("");
    }

    static void displaySummary()
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
                ":" + contacts[position].ContactMinutesSpent + "\n";
        }

        SnapsEngine.SetTitleString("Contact Times");

        SnapsEngine.DisplayString(result);

        SnapsEngine.WaitForButton("Continue");

        SnapsEngine.DisplayString("");
    }

    public static void StartProgram()
    {


        ////Use these statements to make test data
        //makeTestData();
        //storeAllContacts();

        loadAllContacts();

        while (true)
        {
            // Clear the display 
            SnapsEngine.SetTitleString("Time Tracker");
            SnapsEngine.DisplayString("");

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
