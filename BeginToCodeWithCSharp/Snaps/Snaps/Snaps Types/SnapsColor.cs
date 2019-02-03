namespace SnapsLibrary
{
    public struct SnapsColor
    {
        public byte RedValue;
        public byte GreenValue;
        public byte BlueValue;

        public SnapsColor(byte red, byte green, byte blue)
        {
            RedValue = red;
            GreenValue = green;
            BlueValue = blue;
        }

        public static SnapsColor Red = new SnapsColor(255, 0, 0);
        public static SnapsColor Green = new SnapsColor(0, 255, 0);
        public static SnapsColor Blue = new SnapsColor(0, 0, 255);
        public static SnapsColor Yellow = new SnapsColor(255, 255, 0);
        public static SnapsColor Magenta = new SnapsColor(255, 0, 255);
        public static SnapsColor Cyan = new SnapsColor(0, 255, 255);
        public static SnapsColor Black = new SnapsColor(0, 0, 0);
        public static SnapsColor White = new SnapsColor(255, 255, 255);

        private static System.Random rand = new System.Random();

        public static SnapsColor Random
        {
            get
            {
                return new SnapsColor((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            }
        }
    }
}
