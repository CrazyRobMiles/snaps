using SnapsLibrary;

class Ch06_07_TapScreenToStop
{
    public void StartProgram()
    {
        SnapsEngine.SetTitleString("Talking Times Tables");

        while (true)
        {
            int timesValue = 2;

            // Make sure that the screen tapped flag is clear
            SnapsEngine.ClearScreenTappedFlag();

            for (int count = 1; count < 13; count = count + 1)
            {

                int result = count * timesValue;

                string message = count.ToString() +
                    " times " + timesValue.ToString() +
                    " is " + result.ToString();

                SnapsEngine.DisplayString(message);
                SnapsEngine.SpeakString(message);

                // If the screen is tapped, break out of the for loop
                if (SnapsEngine.ScreenHasBeenTapped())
                    break;
            }
            SnapsEngine.WaitForButton("Press to continue");
        }
    }
}