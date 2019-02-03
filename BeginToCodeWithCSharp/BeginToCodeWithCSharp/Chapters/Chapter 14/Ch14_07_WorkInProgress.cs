using SnapsLibrary;

using System;
using System.Collections.Generic;

public class Ch14_07_WorkInProgress
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
        

        public List<MissileSprite> Missiles = new List<MissileSprite>();

        public RocketSprite(ImageSprite sprite, double xSpeed, double ySpeed) : base(sprite, xSpeed, ySpeed)
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

            if (SnapsEngine.GetFireGamepad())
                // Work through all the missiles
                foreach (MissileSprite missile in Missiles)
                    if (!missile.IsFlying)
                        // if the missile is not flying we can launch it
                        missile.FireMissile(spriteValue.CenterX, spriteValue.CenterY);
        }
    }

    public class AlienSprite : MovingSprite
    {
        public bool AlienAlive = true;
        public RocketSprite rocketValue;

        public AlienSprite(ImageSprite sprite, double xSpeed, double ySpeed, RocketSprite target) :
            base(sprite: sprite, xSpeed: xSpeed, ySpeed: ySpeed)
        {
            rocketValue = target;
        }

        public void Kill()
        {
            if (AlienAlive)
            {
                AlienAlive = false;
                spriteValue.Hide();
            }
        }

        public override void Update()
        {
            // don't do anything if the alien is dead
            if (!AlienAlive)
                return;

            // Update the position of the sprite
            base.Update();
        }
    }

    public class ChasingAlien : AlienSprite
    {
        public double xAccelerationValue;
        public double yAccelerationValue;
        public double frictionValue;

        public ChasingAlien(ImageSprite sprite, RocketSprite target,
        double xAcceleration, double yAcceleration, double friction) :
        base(sprite: sprite, xSpeed: 0, ySpeed: 0, target: target)
        {
            rocketValue = target;
            xAccelerationValue = xAcceleration;
            yAccelerationValue = yAcceleration;
            frictionValue = friction;
        }

        public override void Update()
        {
            base.Update();
            if (AlienAlive)
            {
                if (rocketValue.spriteValue.CenterX > spriteValue.CenterX)
                    xSpeedValue = xSpeedValue + xAccelerationValue;
                else
                    xSpeedValue = xSpeedValue - xAccelerationValue;

                xSpeedValue = xSpeedValue * frictionValue;

                if (rocketValue.spriteValue.CenterY > spriteValue.CenterY)
                    ySpeedValue = ySpeedValue + yAccelerationValue;
                else
                    ySpeedValue = ySpeedValue - yAccelerationValue;

                ySpeedValue = ySpeedValue * frictionValue;
            }
        }
    }

    public class LineAlien : AlienSprite
    {
        public double xMaxValue, xMinValue;

        public LineAlien(ImageSprite sprite, double xSpeed, double ySpeed, RocketSprite target, double xMax, double xMin) :
            base(sprite: sprite, xSpeed: xSpeed, ySpeed: ySpeed, target: target)
        {
            xMinValue = xMin;
            xMaxValue = xMax;
        }

        public override void Update()
        {
            base.Update();

            if (AlienAlive)
            {
                if (spriteValue.X > xMaxValue)
                {
                    spriteValue.X = xMaxValue;
                    xSpeedValue = -xSpeedValue;
                }
                if (spriteValue.X < xMinValue)
                {
                    spriteValue.X = xMinValue;
                    xSpeedValue = -xSpeedValue;
                }
            }
        }
    }

    public class MissileSprite : MovingSprite
    {
        List<AlienSprite> aliensValue;

        RocketSprite rocketValue;

        public bool IsFlying = false;

        public MissileSprite(ImageSprite sprite, RocketSprite rocket, double xSpeed, double ySpeed, List<AlienSprite> aliens) : base(sprite: sprite, xSpeed: xSpeed, ySpeed: ySpeed)
        {
            rocketValue = rocket;
            aliensValue = aliens;
            spriteValue.Hide();
        }

        public override void Update()
        {
            if (!IsFlying)
                return;

            base.Update();  // move the missle

            foreach (AlienSprite alien in aliensValue)
            {
                if (!alien.AlienAlive)
                    continue;

                if (spriteValue.IntersectsWith(alien.spriteValue))
                {
                    // just killed an alien 
                    // get the score for the kill
                    alien.Kill();
                    // remove the missile from the screen
                    spriteValue.Hide();
                    IsFlying = false;
                }
            }
            if (spriteValue.Bottom < 0)
            {
                // missile has gone off the top of the screen
                IsFlying = false;
                spriteValue.Hide();
            }
        }

        public void FireMissile(double missileX, double missileY)
        {
            if (IsFlying)
                return;
            spriteValue.CenterX = missileX;
            spriteValue.CenterY = missileY;
            spriteValue.Show();
            IsFlying = true;
        }
    }


    public void StartProgram()
    {
        

        SnapsEngine.SetBackgroundColor(SnapsColor.Black);

        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        List<MovingSprite> sprites = new List<MovingSprite>();

        List<AlienSprite> aliens = new List<AlienSprite>();

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

        ImageSprite chasingAlienImage = new ImageSprite(imageURL: "ms-appx:///Images/purpleAlien.png");
        SnapsEngine.AddSpriteToGame(chasingAlienImage);
        chasingAlienImage.Top = 10;
        chasingAlienImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 20);
        chasingAlienImage.CenterX = SnapsEngine.GameViewportWidth / 2.0;
        chasingAlienImage.Top = 0;
        ChasingAlien chaser = new ChasingAlien(sprite: chasingAlienImage, target: rocket, xAcceleration: .3, yAcceleration: .3, friction: 0.99);
        sprites.Add(chaser);
        aliens.Add(chaser);

        int noOfAliens = 10;

        double alienWidth = SnapsEngine.GameViewportWidth / (noOfAliens * 2);
        double alienSpacing = (SnapsEngine.GameViewportWidth - alienWidth) / noOfAliens;
        double alienX = 0;
        double alienY = 100;
        for (int i = 0; i < noOfAliens; i = i + 1)
        {
            ImageSprite alienImage = new ImageSprite(imageURL: "ms-appx:///Images/greenAlien.png");
            SnapsEngine.AddSpriteToGame(alienImage);
            alienImage.ScaleSpriteWidth(alienWidth);
            alienImage.CenterX = alienX;
            alienImage.Top = alienY;
            double xMin = alienX;
            double xMax = alienX + alienSpacing;
            LineAlien alien = new LineAlien(sprite: alienImage, xSpeed: 2, ySpeed: 0, target: rocket, xMax: xMax, xMin: xMin);
            sprites.Add(alien);
            aliens.Add(alien);
            alienX = alienX + alienSpacing;
        }

        ImageSprite missileImage = new ImageSprite(imageURL: "ms-appx:///Images/Missile.png");
        missileImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 200);
        SnapsEngine.AddSpriteToGame(missileImage);
        MissileSprite missile = new MissileSprite(sprite: missileImage, rocket: rocket, xSpeed: 0, ySpeed: -15, aliens: aliens);
        sprites.Add(missile);

        rocket.Missiles.Add(missile);

        while (true)
        {
            foreach (MovingSprite sprite in sprites)
                sprite.Update();
            SnapsEngine.DrawGamePage();
        }
    }
}

