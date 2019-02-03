using SnapsLibrary;

class Ch09_06_ConstructingSongNotes
{
    public struct SongNote
    {
        public int Pitch;
        public double Duration;

        public SongNote(int inPitch, double inDuration)
        {
            Pitch = inPitch;
            Duration = inDuration;
            SnapsEngine.DisplayDialog("Hello from the SongNote constructor");
        }
    }

    public void StartProgram()
    {
        SongNote note = new SongNote(inPitch: 0, inDuration: 0.4);
    }
}
