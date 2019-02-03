using SnapsLibrary;

class Ch08_08_TinyContactBookLocalStore
{
    /// <summary>
    /// Tidies up a contact name for use in a search 
    /// </summary>
    /// <param name="input">name to be tidied up</param>
    /// <returns>tidied contact name</returns>
    string TidyInput(string input)
    {
        input = input.Trim();
        input = input.ToLower();
        return input;
    }

    /// <summary>
    /// Fetches a contact from local store
    /// </summary>
    /// <param name="name">name to search for</param>
    /// <param name="address">address that was found - null if not found</param>
    /// <param name="phone">phone that was found - null if not found</param>
    /// <returns>false if the contact was not found</returns>
    bool FetchContact(string name, out string address, out string phone)
    {
        name = TidyInput(name);

        address = SnapsEngine.FetchStringFromLocalStorage(itemName: name + ":address");
        phone = SnapsEngine.FetchStringFromLocalStorage(itemName: name + ":phone");

        if (address == null || phone == null) return false;

        return true;
    }

    /// <summary>
    /// Stores a contact in local store
    /// </summary>
    /// <param name="name">name to use to label the item</param>
    /// <param name="address">address to store</param>
    /// <param name="phone">phone to store</param>
    void StoreContact(string name, string address, string phone)
    {
        name = TidyInput(name);

        SnapsEngine.SaveStringToLocalStorage(itemName: name + ":address",
                                            itemValue: address);
        SnapsEngine.SaveStringToLocalStorage(itemName: name + ":phone",
                                            itemValue: phone);
    }

    /// <summary>
    /// Asks the user for contact details and then stores them
    /// </summary>
    void NewContact()
    {
        SnapsEngine.DisplayString("Enter the contact");
        string name = SnapsEngine.ReadString("Enter new contact name");
        string address = SnapsEngine.ReadMultiLineString("Enter contact address");
        string phone = SnapsEngine.ReadString("Enter contact phone");
        StoreContact(name: name, address: address, phone: phone);
    }

    /// <summary>
    /// Asks the user for a contact name and displays it
    /// </summary>
void FindContact()
{
    // Get the name of the contact to search for
    string name = SnapsEngine.ReadString("Enter contact name");

    // Variables to hold the names address being fetched
    string contactAddress, contactPhone;

    if (FetchContact(name: name, address: out contactAddress, phone: out contactPhone))
    {
        // Got the contact details - display them
        SnapsEngine.ClearTextDisplay();

        SnapsEngine.AddLineToTextDisplay("Name: " + name);
        SnapsEngine.AddLineToTextDisplay("Address: " + contactAddress);
        SnapsEngine.AddLineToTextDisplay("Phone: " + contactPhone);

    }
    else
    {
        // Tell the user the name was not found
        SnapsEngine.DisplayString("Name not found");
    }

    // Give the user a chance to view the details
    SnapsEngine.WaitForButton("Continue");

    // Clear the display
    SnapsEngine.ClearTextDisplay();
}

    /// <summary>
    /// Program entry point
    /// </summary>
    public void StartProgram()
    {
        while (true)
        {
            SnapsEngine.SetTitleString("Tiny Contacts");

            string command = SnapsEngine.SelectFrom2Buttons("New Contact", "Find Contact");

            if (command == "New Contact")
            {
                NewContact();
            }

            if (command == "Find Contact")
            {
                FindContact();
            }
        }
    }
}
