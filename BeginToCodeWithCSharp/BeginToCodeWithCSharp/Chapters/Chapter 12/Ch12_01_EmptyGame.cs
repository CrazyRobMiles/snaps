using SnapsLibrary;

public class Ch12_01_EmptyGame
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        while (true)
        {
            // Game update logic goes here
            SnapsEngine.DrawGamePage();
        }
    }
}
