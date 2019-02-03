using SnapsLibrary;

class Ch05_17_CityImage
{
    public void StartProgram()
    {
        string url = "ms-appx:///Images/City.jpg";
        SnapsEngine.DisplayImageFromUrl(imageURL: url);
    }
}

