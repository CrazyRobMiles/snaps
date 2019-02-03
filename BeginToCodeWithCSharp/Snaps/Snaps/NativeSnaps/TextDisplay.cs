namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        static string textDisplayLine = "";

        public static void ClearTextDisplay()
        {
            textDisplayLine = "";
            DisplayString(textDisplayLine);
        }

        public static void AddTextToTextDisplay(string text)
        {
            textDisplayLine = textDisplayLine + text;
            DisplayString(textDisplayLine);
        }

        public static void AddLineToTextDisplay(string text)
        {
            textDisplayLine = textDisplayLine + text + "\n";
            DisplayString(textDisplayLine);
        }
    }
}