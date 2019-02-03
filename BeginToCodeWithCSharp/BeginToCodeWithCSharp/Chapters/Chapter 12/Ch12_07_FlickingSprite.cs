using SnapsLibrary;

public class Ch12_07_FlickingSprite
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite ball = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(ball);

        double ballWidth = SnapsEngine.GameViewportWidth / 20.0;

        ball.ScaleSpriteWidth(ballWidth);

        while (true)
        {
            ball.X = 0;
            ball.Y = 0;
            SnapsEngine.DrawGamePage();
            SnapsEngine.Delay(0.5);
            ball.X = 500;
            ball.Y = 500;
            SnapsEngine.DrawGamePage();
            SnapsEngine.Delay(0.5);
        }
    }
}

