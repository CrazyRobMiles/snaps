using SnapsLibrary;
using System;

class Ch11_02_PrivateMinutesSpent
{
    class Contact
    {
        public string ContactName;
        public string ContactAddress;
        public string ContactPhone;

        private int contactMinutesSpent;

        public Contact(string name, string address, string phone)
        {
            this.ContactName = name;
            this.ContactAddress = address;
            this.ContactPhone = phone;
            this.contactMinutesSpent = 0;
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
    }

    public static void StartProgram()
    {
        Contact moresecure = new Contact("Rob", "Rob's House", "Rob's Phone");
        int robsMinutes = moresecure.GetMinutesSpent();
        robsMinutes = robsMinutes + 10;
        moresecure.SetMinutesSpent(robsMinutes);
    }
}