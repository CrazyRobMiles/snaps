using SnapsLibrary;

class Ch04_11_Dice
{
    public void StartProgram()
    {
        int spotCount = SnapsEngine.ThrowDice();

        SnapsEngine.SetTitleString(spotCount.ToString());

        SnapsEngine.SpeakString("You have rolled a " + spotCount.ToString());
    }
}