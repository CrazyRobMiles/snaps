using SnapsLibrary;

public class Ch12_06_BallSpriteFullScreen
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite scaledBall = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(scaledBall);

        scaledBall.Width = SnapsEngine.GameViewportWidth;
        scaledBall.Height = SnapsEngine.GameViewportHeight;

        while (true)
        {
            SnapsEngine.DrawGamePage();
        }
    }
}
