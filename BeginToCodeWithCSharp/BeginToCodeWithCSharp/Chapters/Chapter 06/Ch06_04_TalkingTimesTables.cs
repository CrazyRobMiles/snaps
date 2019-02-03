using SnapsLibrary;

class Ch06_04_TalkingTimesTables
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Talking Times Tables");

        int count = 1;

        int timesValue = 2;

        while (count < 13)
        {
            int result = count * timesValue;

            string message = count.ToString() +
                " times " + timesValue.ToString() +
                " is " + result.ToString();

            SnapsEngine.DisplayString(message);
            SnapsEngine.SpeakString(message);

            count = count + 1;
        }
    }
}
