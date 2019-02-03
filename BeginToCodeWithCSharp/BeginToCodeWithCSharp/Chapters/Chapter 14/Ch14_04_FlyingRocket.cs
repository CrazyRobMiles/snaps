using SnapsLibrary;

using System;
using System.Collections.Generic;

public class Ch14_04_FlyingRocket
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

    public class RocketSprite : MovingSprite
    {
        
        public RocketSprite(ImageSprite sprite, double xSpeed, double ySpeed) : base(sprite:sprite, xSpeed:xSpeed, ySpeed:ySpeed)
        {
        }

        public override void Update()
        {
            if (SnapsEngine.GetUpGamepad())
                spriteValue.Y = spriteValue.Y - ySpeedValue;

            if (SnapsEngine.GetDownGamepad())
                spriteValue.Y = spriteValue.Y + ySpeedValue;

            if (SnapsEngine.GetRightGamepad())
                spriteValue.X = spriteValue.X + xSpeedValue;

            if (SnapsEngine.GetLeftGamepad())
                spriteValue.X = spriteValue.X - xSpeedValue;

            if (spriteValue.Left < 0)
                spriteValue.Left = 0;

            if (spriteValue.Right > SnapsEngine.GameViewportWidth)
                spriteValue.Right = SnapsEngine.GameViewportWidth;

            if (spriteValue.Top < 0)
                spriteValue.Top = 0;

            if (spriteValue.Bottom > SnapsEngine.GameViewportHeight)
                spriteValue.Bottom = SnapsEngine.GameViewportHeight;
        }
    }

    public void StartProgram()
    {
        SnapsEngine.SetBackgroundColor(SnapsColor.Black);

        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        List<MovingSprite> sprites = new List<MovingSprite>();

        for (int i = 0; i < 100; i++)
        {
            ImageSprite starImage = new ImageSprite(imageURL: "ms-appx:///Images/star.png");
            SnapsEngine.AddSpriteToGame(starImage);
            starImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 75);
            FallingSprite star = new FallingSprite(sprite: starImage,ySpeed: 15);
            sprites.Add(star);
        }

        ImageSprite rocketImage = new ImageSprite(imageURL: "ms-appx:///Images/SpaceRocket.png");
        SnapsEngine.AddSpriteToGame(rocketImage);
        rocketImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 15);
        rocketImage.CenterX = SnapsEngine.GameViewportWidth / 2.0;
        rocketImage.CenterY = SnapsEngine.GameViewportHeight / 2.0;

        RocketSprite rocket = new RocketSprite(sprite: rocketImage,  xSpeed: 10, ySpeed: 10);
        sprites.Add(rocket);

        while (true)
        {
            foreach (MovingSprite sprite in sprites)
                sprite.Update();
            SnapsEngine.DrawGamePage();
        }
    }
}

