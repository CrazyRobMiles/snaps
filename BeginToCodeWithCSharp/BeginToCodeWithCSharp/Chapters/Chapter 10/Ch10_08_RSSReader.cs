using SnapsLibrary;
using System.Xml.Linq;

class Ch10_08_RSSReader
{
    public static void StartProgram()
    {
        string rssText = SnapsEngine.GetWebPageAsString("http://www.robmiles.com/?format=rss");

        XElement rssElements = XElement.Parse(rssText);

        string title = rssElements.Element("channel").Element("item").Element("title").Value;

        SnapsEngine.SetTitleString("Headline from Rob");
        SnapsEngine.DisplayString(title);
    }
}
