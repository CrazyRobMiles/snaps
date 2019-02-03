using SnapsLibrary;

using System;

public class Ch14_02_SingleFallingStar
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

        public virtual void Update()
        {
            spriteValue.X = spriteValue.X + xSpeedValue;
            spriteValue.Y = spriteValue.Y + ySpeedValue;
        }
    }

    public class FallingSprite : MovingSprite
    {
        static Random spriteRand = new Random();

        public FallingSprite(ImageSprite sprite, double ySpeed) :
                base(sprite: sprite, xSpeed: 0, ySpeed: ySpeed)
        {
            spriteValue.Left = (SnapsEngine.GameViewportHeight - spriteValue.Width) * spriteRand.NextDouble();
            spriteValue.Bottom = SnapsEngine.GameViewportHeight * spriteRand.NextDouble();
        }

        public override void Update()
        {
            base.Update();

            if (spriteValue.Top > SnapsEngine.GameViewportHeight)
            {
                spriteValue.Left = (SnapsEngine.GameViewportWidth - spriteValue.Width) * spriteRand.NextDouble();
                spriteValue.Bottom = 0;
            }
        }
    }

    public void StartProgram()
    {
        SnapsEngine.SetBackgroundColor(SnapsColor.Black);

        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite starImage = new ImageSprite(imageURL: "ms-appx:///Images/star.png");
        SnapsEngine.AddSpriteToGame(starImage);
        starImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 50);
        FallingSprite star = new FallingSprite(sprite: starImage, ySpeed: 1);

        while (true)
        {
            star.Update();
            SnapsEngine.DrawGamePage();
        }
    }
}




