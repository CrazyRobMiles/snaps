using SnapsLibrary;

public class Ch14_01_MovingSprite
{
    public class MovingSprite
    {
        public ImageSprite spriteValue;
        public double xSpeedValue, ySpeedValue;

        public MovingSprite(ImageSprite sprite, double xSpeed, double ySpeed)
        {
            spriteValue = sprite;
            xSpeedValue = xSpeed;
            ySpeedValue = ySpeed;
        }

        public void Update()
        {
            spriteValue.X = spriteValue.X + xSpeedValue;
            spriteValue.Y = spriteValue.Y + ySpeedValue;
        }
    }

    public void StartProgram()
    {
        SnapsEngine.SetBackgroundColor(SnapsColor.Black);

        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite starImage = new ImageSprite(imageURL: "ms-appx:///Images/star.png");
        SnapsEngine.AddSpriteToGame(starImage);
        starImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 50);
        MovingSprite star = new MovingSprite(sprite: starImage, xSpeed: 0, ySpeed: 1);

        while (true)
        {
            star.Update();
            SnapsEngine.DrawGamePage();
        }
    }
}

