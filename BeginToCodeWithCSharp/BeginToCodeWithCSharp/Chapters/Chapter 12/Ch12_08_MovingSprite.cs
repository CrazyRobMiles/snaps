using SnapsLibrary;

public class Ch12_08_MovingSprite
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite ball = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(ball);

        double ballWidth = SnapsEngine.GameViewportWidth / 20.0;

        ball.ScaleSpriteWidth(ballWidth);

        double XBallSpeed = 1;
        double YBallSpeed = 1;

        while (true)
        {
            ball.X = ball.X + XBallSpeed;
            ball.Y = ball.Y + YBallSpeed;
            SnapsEngine.DrawGamePage();
        }
    }
}

