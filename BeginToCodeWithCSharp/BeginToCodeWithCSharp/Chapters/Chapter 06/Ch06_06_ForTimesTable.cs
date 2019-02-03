using SnapsLibrary;

class Ch06_06_ForTimesTable
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Talking Times Tables");

        while (true)
        {
            int timesValue = 2;

            for (int count = 1; count < 13; count = count + 1)
            {
                int result = count * timesValue;

                string message = count.ToString() +
                    " times " + timesValue.ToString() +
                    " is " + result.ToString();

                SnapsEngine.DisplayString(message);
                SnapsEngine.SpeakString(message);
            }
            SnapsEngine.WaitForButton("Press to continue");
        }
    }
}
