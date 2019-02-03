using SnapsLibrary;
using System;
using System.Collections.Generic;

public class Ch15_05_InterfaceSprites
{
    interface IGameSprite
    {
        void Reset();
        void Update();
    }

    public abstract class MovingSprite: IGameSprite
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

        public abstract void Reset();  
    }

    public class FallingSprite : MovingSprite
    {
        static Random spriteRand = new Random();

        double viewportWidthValue, viewportHeightValue;

        double originalX, originalY;

        public override void Reset()
        {
            spriteValue.X = originalX;
            spriteValue.Y = originalY;
        }

        public FallingSprite(ImageSprite sprite, double xSpeed, double ySpeed, double viewportWidth, double viewportHeight) :
                base(sprite: sprite, xSpeed: xSpeed, ySpeed: ySpeed)
        {
            viewportWidthValue = viewportWidth;
            viewportHeightValue = viewportHeight;
            spriteValue.Left = (viewportWidthValue - spriteValue.Width) * spriteRand.NextDouble();
            spriteValue.Bottom = viewportHeightValue * spriteRand.NextDouble();
            originalX = sprite.X;
            originalY = sprite.Y;
        }

        public void resetSprite()
        {
            spriteValue.Left = (viewportWidthValue - spriteValue.Width) * spriteRand.NextDouble();
            spriteValue.Bottom = 0;
        }

        public override void Update()
        {
            base.Update();

            if (spriteValue.Top > viewportHeightValue)
            {
                spriteValue.Left = (viewportWidthValue - spriteValue.Width) * spriteRand.NextDouble();
                spriteValue.Bottom = 0;
            }
        }
    }

    public class RocketSprite : MovingSprite
    {
        

        SpaceRocketsInSpaceGame gameValue;

        public List<MissileSprite> Missiles = new List<MissileSprite>();

        double originalX, originalY;

        public int LivesLeft, Score;

        public override void Reset()
        {
            spriteValue.X = originalX;
            spriteValue.Y = originalY;
            LivesLeft = 3;
            Score = 0;
        }

        public RocketSprite(ImageSprite sprite,
                            
                            SpaceRocketsInSpaceGame game,
                            double xSpeed, double ySpeed) :
                            base(sprite, xSpeed, ySpeed)
        {
            originalX = sprite.X;
            originalY = sprite.Y;
            
            gameValue = game;
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

        public void TakeDamage()
        {
            LivesLeft = LivesLeft - 1;
            SnapsEngine.PlayGameSoundEffect("ding");

            if (LivesLeft == 0)
            {
                gameValue.EndCurrentGame();
            }
        }

    }

    public class AlienSprite : MovingSprite
    {
        public bool AlienAlive = true;
        public RocketSprite rocketValue;
        public SpaceRocketsInSpaceGame gameValue;

        double originalX, originalY;

        public override void Reset()
        {
            spriteValue.X = originalX;
            spriteValue.Y = originalY;
            AlienAlive = true;
            spriteValue.Show();
        }

        public AlienSprite(ImageSprite sprite, SpaceRocketsInSpaceGame game, double xSpeed, double ySpeed, RocketSprite target) :
            base(sprite: sprite, xSpeed: xSpeed, ySpeed: ySpeed)
        {
            gameValue = game;
            originalX = sprite.X;
            originalY = sprite.Y;
            rocketValue = target;
        }

        public void Kill()
        {
            if (AlienAlive)
            {
                gameValue.AddScoredPoints(50);
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

            if (spriteValue.IntersectsWith(rocketValue.spriteValue))
            {
                AlienAlive = false;
                rocketValue.TakeDamage();
                spriteValue.Hide();
                return;
            }
        }
    }

    public class ChasingAlien : AlienSprite
    {
        public double xAccelerationValue;
        public double yAccelerationValue;
        public double frictionValue;

        public ChasingAlien(ImageSprite sprite, SpaceRocketsInSpaceGame game, 
            RocketSprite target,
            double xAcceleration, double yAcceleration, double friction) :
        base(sprite: sprite, game: game, xSpeed: 0, ySpeed: 0, target: target)
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

        public LineAlien(ImageSprite sprite, SpaceRocketsInSpaceGame game, double xSpeed, double ySpeed, RocketSprite target, double xMax, double xMin) :
            base(sprite: sprite, game: game, xSpeed: xSpeed, ySpeed: ySpeed, target: target)
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

        public override void Reset()
        {
            IsFlying = false;
        }

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

    public class SpaceRocketsInSpaceGame
    {
        

        List<IGameSprite> sprites = new List<IGameSprite>();

        List<AlienSprite> aliens = new List<AlienSprite>();

        RocketSprite rocket;

        ChasingAlien chaser;

        ImageSprite titleScreen;

        ImageSprite gameOverScreen;

        TextBlockSprite messageBack, message;

        enum GameStates
        {
            TitleScreen,
            GameActive,
            GameOver
        }

        GameStates state = GameStates.TitleScreen;

        void resetGame()
        {
            gameScore = 0;
            foreach (MovingSprite sprite in sprites)
                sprite.Reset();
        }

        int gameScore;

        public void AddScoredPoints(int points)
        {
            gameScore = gameScore + points;
        }

        int gameOverTimer;

        public void EndCurrentGame()
        {
            titleScreen.Hide();
            gameOverScreen.Show();
            state = GameStates.GameOver;
            gameOverTimer = 0;
        }

        public void StartNewGame()
        {
            resetGame();
            gameOverScreen.Hide();
            titleScreen.Hide();
            state = GameStates.GameActive;
        }

        public void ShowTitleScreen()
        {
            gameOverScreen.Hide();
            titleScreen.Show();
            state = GameStates.TitleScreen;
        }

        public void UpdateGame()
        {
            foreach (MovingSprite sprite in sprites)
                sprite.Update();

            string status = "Score: " + gameScore.ToString() + 
                " Lives: " + rocket.LivesLeft.ToString();
            message.Text = status;
            messageBack.Text = status;

            messageBack.Bottom = SnapsEngine.GameViewportHeight - 74;
            messageBack.CenterX = SnapsEngine.GameViewportWidth / 2;

            message.Bottom = SnapsEngine.GameViewportHeight - 70;
            message.CenterX = SnapsEngine.GameViewportWidth / 2 + 4;

            bool allAliensDead = true;

            foreach (AlienSprite alien in aliens)
                if(alien.AlienAlive)
                {
                    allAliensDead = false;
                    break;
                }

            if(allAliensDead)
            {
                foreach (AlienSprite alien in aliens)
                {
                    alien.Reset();
                }
            }
        }

        public void UpdateTitle()
        {
            if (SnapsEngine.GetFireGamepad())
            {
                StartNewGame();
            }
        }

        public void UpdateGameOver()
        {
            gameOverTimer = gameOverTimer + 1;
            if (gameOverTimer > 300)
            {
                ShowTitleScreen();
            }
        }

        void setupGame()
        {
            

            SnapsEngine.SetBackgroundColor(SnapsColor.Black);

            SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

            for (int i = 0; i < 100; i++)
            {
                ImageSprite starImage = new ImageSprite(imageURL: "ms-appx:///Images/star.png");
                SnapsEngine.AddSpriteToGame(starImage);
                starImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 75);
                FallingSprite star = new FallingSprite(sprite: starImage,
                    xSpeed: 0, ySpeed: 15,
                    viewportWidth: SnapsEngine.GameViewportWidth,
                    viewportHeight: SnapsEngine.GameViewportHeight);
                sprites.Add(star);
            }

            ImageSprite rocketImage = new ImageSprite(imageURL: "ms-appx:///Images/SpaceRocket.png");
            SnapsEngine.AddSpriteToGame(rocketImage);
            rocketImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 15);
            rocketImage.CenterX = SnapsEngine.GameViewportWidth / 2.0;
            rocketImage.CenterY = SnapsEngine.GameViewportHeight / 2.0;

            rocket = new RocketSprite(sprite: rocketImage,  game: this, xSpeed: 10, ySpeed: 10);
            sprites.Add(rocket);

            ImageSprite chasingAlienImage = new ImageSprite(imageURL: "ms-appx:///Images/purpleAlien.png");
            SnapsEngine.AddSpriteToGame(chasingAlienImage);
            chasingAlienImage.Top = 10;
            chasingAlienImage.ScaleSpriteWidth(SnapsEngine.GameViewportWidth / 20);
            chasingAlienImage.CenterX = SnapsEngine.GameViewportWidth / 2.0;
            chasingAlienImage.Top = 0;
            chaser = new ChasingAlien(sprite: chasingAlienImage, game: this, target: rocket, xAcceleration: .3, yAcceleration: .3, friction: 0.99);
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
                LineAlien alien = new LineAlien(sprite: alienImage, game: this, xSpeed: 2, ySpeed: 0, target: rocket, xMax: xMax, xMin: xMin);
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

            gameOverScreen = new ImageSprite(imageURL: "ms-appx:///Images/SpaceRocketsInSpaceGameOverScreen.png");
            gameOverScreen.Hide();
            SnapsEngine.AddSpriteToGame(gameOverScreen);
            gameOverScreen.Width = SnapsEngine.GameViewportWidth;
            gameOverScreen.Height = SnapsEngine.GameViewportHeight;

            messageBack = new TextBlockSprite(text: "", fontSize: 60,
                fontFamily: "Impact", color: SnapsColor.Black);
            SnapsEngine.AddSpriteToGame(messageBack);
            message = new TextBlockSprite(text: "", fontSize: 60,
                fontFamily: "Impact", color: SnapsColor.Red);
            SnapsEngine.AddSpriteToGame(message);

            titleScreen = new ImageSprite(imageURL: "ms-appx:///Images/SpaceRocketsInSpaceTitleScreen.png");
            titleScreen.Hide();
            SnapsEngine.AddSpriteToGame(titleScreen);
            titleScreen.Width = SnapsEngine.GameViewportWidth;
            titleScreen.Height = SnapsEngine.GameViewportHeight;

        }

        public void PlayGame()
        {
            setupGame();

            resetGame();

            ShowTitleScreen();

            while (true)
            {
                switch (state)
                {
                    case GameStates.TitleScreen:
                        UpdateTitle();
                        break;
                    case GameStates.GameActive:
                        UpdateGame();
                        break;
                    case GameStates.GameOver:
                        UpdateGameOver();
                        break;
                }
                SnapsEngine.DrawGamePage();
            }
        }
    }

    public void StartProgram()
    {
        SpaceRocketsInSpaceGame game = new SpaceRocketsInSpaceGame();

        game.PlayGame();
    }
}

