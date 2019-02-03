using SnapsLibrary;

public class Ch02_01_MysteryProgram1
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Mystery Program 1");

        float inputNumber = SnapsEngine.ReadFloat("Enter a number please");

        if (inputNumber == 40)
            SnapsEngine.DisplayString(@"'Arr. That be my age.' said the Pirate King");
        else
        {
            inputNumber = inputNumber + inputNumber;
            SnapsEngine.DisplayString("Output: " + inputNumber);
        }
    }
}
