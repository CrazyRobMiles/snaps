using SnapsLibrary;

public class Ch12_04_ScaledCity
{

    public void StartProgram()
    {
        SnapsEngine.StartGameEngine(fullScreen: false, framesPerSecond: 60);

        ImageSprite scaledCity = new ImageSprite(imageURL: "ms-appx:///Images/city.jpg");

        SnapsEngine.AddSpriteToGame(scaledCity);

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
            scaledCity.ScaleSpriteWidth(currentWidth);
            SnapsEngine.DrawGamePage();
        }
    }
}
