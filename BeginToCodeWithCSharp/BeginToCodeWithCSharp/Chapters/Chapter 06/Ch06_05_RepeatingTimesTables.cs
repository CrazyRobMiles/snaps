using SnapsLibrary;

class Ch06_05_RepeatingTimesTables
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Talking Times Tables");

        while (true)
        {
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
            SnapsEngine.WaitForButton("Press to continue");
        }
    }
}