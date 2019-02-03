using SnapsLibrary;
using System;

class Ch11_02_GetAndSet
{
    class Contact
    {
        private string contactAddress;
        private string contactPhone;

        private int contactMinutesSpent;

        private string contactName;

        public bool ValidateName(string newName)
        {
            if (newName.Length > 0)
                return true;

            return false;
        }

        public string ContactName
        {
            get
            {
                return contactName;
            }

            set
            {
                if (ValidateName(value))
                    contactName = value;
                else
                    throw new Exception("Invalid name: " + value);
            }
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

        public int GetMinutesSpent()
        {
            return this.contactMinutesSpent;
        }

        public void SetMinutesSpent(int newMinutesSpent)
        {
            if (newMinutesSpent > 0)
                // Only set a value which is greater than 0
                this.contactMinutesSpent = newMinutesSpent;
        }

        public Contact(string name, string address, string phone)
        {
            this.ContactName = name;
            this.ContactAddress = address;
            this.ContactPhone = phone;
            this.contactMinutesSpent = 0;
        }
    }

    public static void StartProgram()
    {
        Contact moresecure = new Contact("Rob", "Rob's House", "Rob's Phone");
        int robsMinutes = moresecure.GetMinutesSpent();
        robsMinutes = robsMinutes + 10;
        moresecure.SetMinutesSpent(robsMinutes);
    }
}