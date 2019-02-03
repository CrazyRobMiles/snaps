using SnapsLibrary;
using System.Xml.Linq;

class Ch10_09_RSSTitles
{
    public static void StartProgram()
    {
        string rssText = SnapsEngine.GetWebPageAsString("http://www.robmiles.com/?format=rss");

        XElement rssElements = XElement.Parse(rssText);

        SnapsEngine.SetTitleString("Headlines from Rob");

        SnapsEngine.ClearTextDisplay();
        foreach ( XElement element in rssElements.Element("channel").Elements("item"))
        {
            SnapsEngine.AddLineToTextDisplay(element.Element("title").Value);
        }
    }
}
