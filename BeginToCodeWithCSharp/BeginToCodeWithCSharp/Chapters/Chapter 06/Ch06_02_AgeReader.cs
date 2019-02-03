using SnapsLibrary;

class Ch06_02_AgeReader
{
    public void StartProgram()
    {
        int age;
        do
        {
            age = SnapsEngine.ReadInteger("Enter your age");
        } while (age < 1 | age > 100);

        SnapsEngine.DisplayString("Your age is " + age);
    }
}