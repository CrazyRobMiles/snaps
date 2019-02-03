using SnapsLibrary;

public class Ch12_02_BallSprite
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite ball = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(ball);

        while (true)
        {
            SnapsEngine.DrawGamePage();
        }
    }
}
