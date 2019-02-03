using SnapsLibrary;

public class Ch02_02_MysteryProgram2
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Mystery Program 2");

        string inputText = SnapsEngine.ReadString("Enter something please");

        if (inputText.Length > 10)
            inputText = inputText.Substring(0, 10);

        SnapsEngine.AddLineToTextDisplay(inputText);

        foreach (char ch in inputText)
        {
            int chVal = (int)ch;
            SnapsEngine.AddLineToTextDisplay("code = " + chVal);
        }
    }
}
