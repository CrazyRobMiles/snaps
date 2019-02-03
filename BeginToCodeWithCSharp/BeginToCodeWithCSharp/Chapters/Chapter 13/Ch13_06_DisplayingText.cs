using SnapsLibrary;

public class Ch13_06_DisplayingText
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        TextBlockSprite tinyTextSprite = new TextBlockSprite(
            text: "Hello. I'm Tiny Text in the default font",
            fontSize: 20, color: SnapsColor.Blue);
        SnapsEngine.AddSpriteToGame(tinyTextSprite);

        TextBlockSprite giantTextSprite = new TextBlockSprite(
            text: "I'm Giant",
            fontSize: 200, fontFamily: "Impact",
            color: SnapsColor.Red);
        SnapsEngine.AddSpriteToGame(giantTextSprite);

        while (true)
        {
            tinyTextSprite.Top = 10;
            tinyTextSprite.CenterX = SnapsEngine.GameViewportWidth / 2.0;

            giantTextSprite.Bottom = SnapsEngine.GameViewportHeight - 10;
            giantTextSprite.CenterX = SnapsEngine.GameViewportWidth / 2.0;

            SnapsEngine.DrawGamePage();
        }
    }
}

