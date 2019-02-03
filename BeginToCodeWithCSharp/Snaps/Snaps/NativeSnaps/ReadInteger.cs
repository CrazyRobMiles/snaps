
namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        public static int ReadInteger(string prompt)
        {
            while(true)
            {
                string intString = SnapsEngine.ReadString(prompt);

                try
                {
                    int result = int.Parse(intString);
                    return result;
                }
                catch
                {
                    SnapsEngine.DisplayDialog("Please enter a number, not text");
                    continue;
                }
            }
        }
    }
}