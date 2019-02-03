using SnapsLibrary;

public class Ch13_08_KeepUpGame
{
    TextBlockSprite titleBack, title;
    TextBlockSprite messageBack, message;

    ImageSprite ball;
    double ballWidth;
    double XBallSpeed;
    double YBallSpeed;

    ImageSprite paddle;

    double XPaddleSpeed;
    double paddleWidth;

    int lives;
    int score;

    private void setupMessages()
    {
        titleBack = new TextBlockSprite(text: "Keep Up!!", fontSize: 320,
            fontFamily: "Impact", color: SnapsColor.Black);
        titleBack.RotationAngle = -20;
        SnapsEngine.AddSpriteToGame(titleBack);
        title = new TextBlockSprite(text: "Keep Up!!", fontSize: 320,
            fontFamily: "Impact", color: SnapsColor.Red);
        title.RotationAngle = -20;
        SnapsEngine.AddSpriteToGame(title);

        messageBack = new TextBlockSprite(text: "Keep Up!!", fontSize: 60,
            fontFamily: "Impact", color: SnapsColor.Black);
        SnapsEngine.AddSpriteToGame(messageBack);
        message = new TextBlockSprite(text: "Keep Up!!", fontSize: 60,
            fontFamily: "Impact", color: SnapsColor.Red);
        SnapsEngine.AddSpriteToGame(message);
    }

    private void setupBall()
    {
        ball = new ImageSprite(imageURL: "ms-appx:///Images/ball.png");
        SnapsEngine.AddSpriteToGame(ball);

        ballWidth = SnapsEngine.GameViewportWidth / 20.0;
        ball.ScaleSpriteWidth(ballWidth);
    }

    private void setupPaddle()
    {
        paddle = new ImageSprite(imageURL: "ms-appx:///Images/paddle.png");
        SnapsEngine.AddSpriteToGame(paddle);
        paddleWidth = SnapsEngine.GameViewportWidth / 10.0;
        paddle.ScaleSpriteWidth(paddleWidth);
    }

    public void setupGame()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);
        setupMessages();
        setupBall();
        setupPaddle();
    }

    public void resetGame()
    {
        lives = 3;
        score = 0;
        XBallSpeed = 10;
        YBallSpeed = 10;
        ball.Top = 0;
        ball.Left = 0;
        paddle.Bottom = SnapsEngine.GameViewportHeight - 10;
        paddle.CenterX = SnapsEngine.GameViewportWidth / 2;
        XPaddleSpeed = 15;
    }


    private void positionMessages()
    {
        titleBack.Top = 10;
        titleBack.CenterX = SnapsEngine.GameViewportWidth / 2;
        title.Top = 18;
        title.CenterX = SnapsEngine.GameViewportWidth / 2 + 8;

        messageBack.Bottom = SnapsEngine.GameViewportHeight - 74;
        messageBack.CenterX = SnapsEngine.GameViewportWidth / 2;

        message.Bottom = SnapsEngine.GameViewportHeight - 70;
        message.CenterX = SnapsEngine.GameViewportWidth / 2 + 4;
    }

    void updateBall()
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

                // If we hit the bottom of the screen we lose a life
                lives = lives - 1;

                // play a sound effect to tell the player the bad news
                SnapsEngine.PlayGameSoundEffect("lose");

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
    }

    private void updateGamepad()
    {
        if (SnapsEngine.GetRightGamepad())
        {
            paddle.X = paddle.X + XPaddleSpeed;
        }

        if (SnapsEngine.GetLeftGamepad())
        {
            paddle.X = paddle.X - XPaddleSpeed;
        }

        if (SnapsEngine.GetFireGamepad())
        {
            paddle.CenterX = ball.CenterX;
        }

        if (paddle.Left < 0)
        {
            // Trying to move off the left edge - pull the pad back
            paddle.Left = 0;
        }

        if (paddle.Right > SnapsEngine.GameViewportWidth)
        {
            // Trying to move off the right edge - pull the pad back
            paddle.Right = SnapsEngine.GameViewportWidth;
        }

        // Handle collisions with the ball

        if (paddle.IntersectsWith(ball))
        {
            if (YBallSpeed > 0)
            {
                // ball is going down, make it bounce off the bat
                // and go up
                YBallSpeed = -YBallSpeed;

                // Make a noise
                SnapsEngine.PlayGameSoundEffect("ding");

                // increase the score
                score = score + 1;

                // move the paddle up the screen
                paddle.Y = paddle.Y - 5;

            }
        }
    }

    private void updateScoreDisplay()
    {
        string status = "Score: " + score.ToString() + " Lives: " + lives.ToString();
        message.Text = status;
        messageBack.Text = status;
    }

    private void displayGameOver()
    {
        message.Text = "Game Over - Score: " + score.ToString();
        messageBack.Text = "Game Over - Score: " + score.ToString();
        SnapsEngine.PlayGameSoundEffect("gameOver");
        SnapsEngine.DrawGamePage();
    }

    private void waitForGameStart()
    {
        ball.Hide();
        paddle.Hide();

        while (true)
        {
            message.Text = "Press Up to play";
            messageBack.Text = "Press Up to play";
            positionMessages();
            SnapsEngine.DrawGamePage();
            if (SnapsEngine.GetUpGamepad())
            {
                break;
            }
        }

        ball.Show();
        paddle.Show();
    }

    public void StartProgram()
    {
        setupGame();

        while (true)
        {
            waitForGameStart();
            resetGame();
            while (true)
            {
                positionMessages();
                updateBall();
                updateGamepad();
                updateScoreDisplay();
                SnapsEngine.DrawGamePage();

                // If we have no lives left we break out of the game loop
                // and end the game
                if (lives == 0)
                    break;
            }

            // when we get here the game is over
            displayGameOver();

            SnapsEngine.Delay(10);
        }
    }
}

