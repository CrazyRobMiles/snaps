using SnapsLibrary;

public class Ch12_03_SquishyBall
{
    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite squishyBall = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(squishyBall);

        float maxWidth = 500;
        float minWidth = 100;
        float currentWidth = 100;
        float widthUpdate = 1;

        while (true)
        {
            currentWidth = currentWidth + widthUpdate;
            if (currentWidth > maxWidth)
                widthUpdate = -1;
            if (currentWidth < minWidth)
                widthUpdate = 1;
            squishyBall.Width = currentWidth;
            SnapsEngine.DrawGamePage();
        }
    }
}
