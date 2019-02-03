using SnapsLibrary;

class Ch06_03_BadAgeReader
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Age between 1 and 100");
        int age;
        do
        {
            age = SnapsEngine.ReadInteger("Enter your age");
        } while (age < 1 & age > 100);
        SnapsEngine.DisplayString("Thank you for entering your age of " + age);
    }
}