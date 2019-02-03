using SnapsLibrary;

public class Ch12_09_BouncingSprite
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ISnapsSprite ball = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");

        SnapsEngine.AddSpriteToGame(ball);

        double ballWidth = SnapsEngine.GameViewportWidth / 20.0;

        ball.ScaleSpriteWidth(ballWidth);

        double XBallSpeed = 10;
        double YBallSpeed = 10;

        while (true)
        {
            // update the ball position according to the speed
            ball.X = ball.X + XBallSpeed;
            ball.Y = ball.Y + YBallSpeed;

            if (ball.Left < 0)
            {
                // ball is going off the left hand edge
                if (XBallSpeed < 0)
                {
                    // ball is moving to the left 
                    // because the speed is negative
                    // make it "bounce" back into the viewport
                    // make the speed positive
                    XBallSpeed = -XBallSpeed;
                }
            }

            if (ball.Right > SnapsEngine.GameViewportWidth)
            {
                // ball is going off the right hand edge
                if (XBallSpeed > 0)
                {
                    // ball is moving to the right
                    // because the speed is positive
                    // make it "bounce" back into the viewport
                    // make the speed negative
                    XBallSpeed = -XBallSpeed;
                }
            }

            if (ball.Bottom > SnapsEngine.GameViewportHeight)
            {
                // ball is going off the bottom edge
                if (YBallSpeed > 0)
                {
                    // ball is moving down the screen
                    // because the speed is positive
                    // make it bounce back into the viewport
                    // make the speed negative
                    YBallSpeed = -YBallSpeed;
                }
            }

            if (ball.Top < 0)
            {
                // ball is going off the top edge
                if (YBallSpeed < 0)
                {
                    // ball is up down the screen
                    // because the speed is negative
                    // make it bounce back into the viewport
                    // make the speed positive
                    YBallSpeed = -YBallSpeed;
                }
            }
            // draw the game page
            SnapsEngine.DrawGamePage();
        }
    }
}

