using SnapsLibrary;

public class Ch13_07_HypnoticText
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        TextBlockSprite hypnoticTextSprite = new TextBlockSprite(
            text: "You are feeling sleepy",
            fontSize: 20, color: SnapsColor.Red);
        SnapsEngine.AddSpriteToGame(hypnoticTextSprite);

        double maxTextSize = 500;
        double minTextSize = 10;
        double textSizeUpdate = 0.2;
        double textSize = minTextSize;

        while (true)
        {
            hypnoticTextSprite.Top = 10;
            hypnoticTextSprite.CenterX = SnapsEngine.GameViewportWidth / 2.0;
            hypnoticTextSprite.CenterY = SnapsEngine.GameViewportHeight / 2.0;
            hypnoticTextSprite.RotationAngle = hypnoticTextSprite.RotationAngle + 1;
            hypnoticTextSprite.FontSize = textSize;

            textSize = textSize + textSizeUpdate;
            if (textSize > maxTextSize || textSize < minTextSize)
            {
                // reverse the direction of the update
                textSizeUpdate = -textSizeUpdate;
            }
            
            SnapsEngine.DrawGamePage();
        }
    }
}

