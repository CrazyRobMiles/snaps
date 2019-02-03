using SnapsLibrary;

public class Ch12_05_BallSpriteTwentieth
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite scaledBall = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(scaledBall);

        double ballWidth = SnapsEngine.GameViewportWidth / 20.0;

        scaledBall.ScaleSpriteWidth(ballWidth);

        while (true)
        {
            SnapsEngine.DrawGamePage();
        }
    }
}

