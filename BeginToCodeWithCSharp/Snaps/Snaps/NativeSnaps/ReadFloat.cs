namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        public static float ReadFloat(string prompt)
        {
            while (true)
            {
                string floatString = ReadString(prompt);

                try
                {
                    float result = float.Parse(floatString);
                    return result;
                }
                catch
                {
                    DisplayDialog("Please enter a number, not text");
                    continue;
                }
            }
        }
    }
}